using SuperMarioOdysseyOnline.Server.Lobby;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Connections;

internal class EventStream
{
    private readonly ILobby _lobby;

    private readonly IPlayer _player;

    private readonly IPacketConnection _connection;

    private readonly IUpdateStrategyFactory _updateStrategyFactory;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public Task Task { get; private set; }

    public EventStream(ILobby lobby, IPlayer player, IPacketConnection connection, IUpdateStrategyFactory updateStrategy)
    {
        _lobby = lobby;
        _player = player;
        _connection = connection;
        _updateStrategyFactory = updateStrategy;

        Task = CreateTask();
    }

    public async ValueTask DisposeAsync()
        => await _cancellationTokenSource.CancelAsync();

    private async Task CreateTask()
    {
        var updateStrategy = await _updateStrategyFactory.CreateAsync(_connection, _cancellationTokenSource.Token);

        await Task.WhenAny(
            ListenForIncomingPacketsAsync(_cancellationTokenSource.Token),
            ListenForUpdatePacketsAsync(updateStrategy, _cancellationTokenSource.Token)
        );
    }

    private async Task ListenForIncomingPacketsAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var packet = await _connection.ReceiveNextPacketAsync(cancellationToken);
            _player.HandleReceivedPacket(packet);
            _lobby.HandleReceivedPacket(packet);
        }
    }

    private async Task ListenForUpdatePacketsAsync(IUpdateStrategy updateStrategy, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var startTime = DateTime.Now;

            var updates = updateStrategy.GetNextUpdateCollection();

            foreach(var update in updates)
            {
                await _connection.SendPacketAsync(update, cancellationToken);
            }

            var executionTimespan = DateTime.Now - startTime;

            if (executionTimespan < updateStrategy.MinimumUpdatePeriod)
            {
                await Task.Delay(updateStrategy.MinimumUpdatePeriod - executionTimespan, cancellationToken);
            }
        }
    }
}
