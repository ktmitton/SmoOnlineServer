using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Connections;

internal class EventStream
{
    private readonly ILobby _lobby;

    private readonly IPlayer _player;

    private readonly IPacketConnection _connection;

    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public Task Task { get; private set; }

    private static readonly object _lock = new();

    public EventStream(ILobby lobby, IPlayer player, IPacketConnection connection)
    {
        _lobby = lobby;
        _player = player;
        _connection = connection;

        Task = CreateTask();
    }

    public async ValueTask DisposeAsync()
        => await _cancellationTokenSource.CancelAsync();

    private async Task CreateTask()
    {
        var updateStrategy = await _lobby.CreateUpdateStrategyAsync(_player, _connection, _cancellationTokenSource.Token);

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
            _lobby.HandleReceivedPacket(_player, packet);
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
                // lock (_lock)
                // {
                //     File.AppendAllText(
                //         $"{_player.Id}.new.log",
                //         $"{_player.Id} [{((PacketType)update.Type).ToString()} {update.Id}]: {string.Join(" ", update.ToByteArray())}\n"
                //     );
                // }
            }

            var executionTimespan = DateTime.Now - startTime;
            Console.WriteLine(executionTimespan);

            if (executionTimespan < updateStrategy.MinimumUpdatePeriod)
            {
                await Task.Delay(updateStrategy.MinimumUpdatePeriod - executionTimespan, cancellationToken);
            }
        }
    }
}
