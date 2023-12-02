using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Events;

public interface IEventStream : IAsyncDisposable
{
    Task InitializeAsync(CancellationToken cancellationToken);

    Task Task { get; }
}

internal class DefaultEventStream(IPlayerConnectionAccessor playerConnectionContextAccessor, IPacketHandler packetHandler) : IEventStream
{
    private readonly IPlayerConnectionAccessor _playerConnectionContextAccessor = playerConnectionContextAccessor;

    private readonly IPacketHandler _packetHandler = packetHandler;

    private CancellationTokenSource? _cancellationTokenSource;

    public Task Task { get; private set; } = Task.CompletedTask;

    public async ValueTask DisposeAsync()
    {
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
        }
    }

    public Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (Interlocked.CompareExchange(ref _cancellationTokenSource, new CancellationTokenSource(), null) is not null)
        {
            throw new Exception("EventStream is already running.");
        }

        Task = Task.WhenAny(
            ListenForIncomingPacketsAsync(_cancellationTokenSource.Token),
            ListenForUpdatePacketsAsync(_cancellationTokenSource.Token)
        );

        return Task.CompletedTask;
    }

    private async Task ListenForIncomingPacketsAsync(CancellationToken cancellationToken)
    {
        var skipCounter = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var connection = _playerConnectionContextAccessor.PlayerConnection;
            var connectedPlayer = connection?.Player;
            if (_playerConnectionContextAccessor.PlayerConnection is null || connectedPlayer is null)
            {
                if (skipCounter >= 12)
                {
                    break;
                }

                ++skipCounter;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

                continue;
            }

            _packetHandler.Handle(
                connectedPlayer,
                await _playerConnectionContextAccessor.PlayerConnection.ReceiveNextPacketAsync(cancellationToken)
            );
        }
    }

    private async Task ListenForUpdatePacketsAsync(CancellationToken cancellationToken)
    {
        var skipCounter = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            var startTime = DateTime.Now;

            var connection = _playerConnectionContextAccessor.PlayerConnection;
            var updateStrategy = connection?.PreferredUpdateStrategy;
            if (connection is null || updateStrategy is null)
            {
                if (skipCounter >= 12)
                {
                    break;
                }

                ++skipCounter;

                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

                continue;
            }

            var updates = updateStrategy.GetNextUpdateCollection();

            foreach(var update in updates)
            {
                await connection.SendPacketAsync(update, cancellationToken);
            }

            var executionTimespan = DateTime.Now - startTime;

            if (executionTimespan < updateStrategy.MinimumUpdatePeriod)
            {
                await Task.Delay(updateStrategy.MinimumUpdatePeriod - executionTimespan, cancellationToken);
            }
        }
    }

    // private async Task SendRemotePlayerUpdatesAsync(IPlayer remotePlayer, CancellationToken cancellationToken)
    // {
    //     var oldLog = _playerUpdateLogs.GetOrAdd(remotePlayer, (x) => {
    //         return new PlayerUpdateLog(
    //             DateTime.MinValue,
    //             (DateTime.MinValue, string.Empty, string.Empty),
    //             (DateTime.MinValue, default),
    //             (DateTime.MinValue, string.Empty),
    //             (DateTime.MinValue, default)
    //         );
    //     });

    //     if (_updateStrategy.ShouldSendMarioLocationUpdate(_connectedPlayer, remotePlayer, oldLog.LastLocationUpdateTimestamp))
    //     {
    //         await stream.WritePacketAsync(new Packet(new MarioLocationData(remotePlayer)),cancellationToken);
    //     }

    //     if (_updateStrategy.ShouldSendCosutmeUpdate(remotePlayer, oldLog.LastCostumeUpdate.MarioCostumeName, oldLog.LastCostumeUpdate.CappyCostumeName, oldLog.LastCostumeUpdate.Timestamp))
    //     {
    //         await stream.WritePacketAsync(new Packet(new CostumeData(remotePlayer)), cancellationToken);
    //     }

    //     if (_updateStrategy.ShouldSendCappyLocationUpdate(remotePlayer, oldLog.LastCappyUpdate.IsThrown, oldLog.LastCappyUpdate.Timestamp))
    //     {
    //         await stream.WritePacketAsync(new Packet(new CappyLocationData(remotePlayer)), cancellationToken);
    //     }

    //     if (_updateStrategy.ShouldSendCaptureUpdate(remotePlayer, oldLog.LastCaptureUpdate.ModelName, oldLog.LastCaptureUpdate.Timestamp))
    //     {
    //         await stream.WritePacketAsync(new Packet(new CaptureData(remotePlayer)), cancellationToken);
    //     }

    //     if (_updateStrategy.ShouldSendStageUpdate(remotePlayer, oldLog.LastStageUpdate.Id, oldLog.LastStageUpdate.Timestamp))
    //     {
    //         await stream.WritePacketAsync(new Packet(new MarioStageData(remotePlayer)), cancellationToken);
    //     }
    // }

    // private record PlayerUpdateLog(
    //     DateTime LastLocationUpdateTimestamp,
    //     (DateTime Timestamp, string MarioCostumeName, string CappyCostumeName) LastCostumeUpdate,
    //     (DateTime Timestamp, bool IsThrown) LastCappyUpdate,
    //     (DateTime Timestamp, string ModelName) LastCaptureUpdate,
    //     (DateTime Timestamp, byte Id) LastStageUpdate
    // );
}
