using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.UpdateStrategies;

public interface IUpdateStrategyFactory
{
    Task<IUpdateStrategy> CreateAsync(IPacketConnection packetConnection, CancellationToken cancellationToken);
}

public interface IUpdateStrategy
{
    /// <summary>
    /// The minimum timespan for an update cycle.
    /// </summary>
    /// <remarks>
    /// If an update finishes withing this timespan, there will be a delay until the next update is executed.
    /// If an update takes longer than this value, the next update occurs immediately.
    /// </remarks>
    TimeSpan MinimumUpdatePeriod { get; }

    /// <summary>
    /// The rating of the connection for this update strategy, where higher numbers represent better matches of a connection to the update strategy.
    /// </summary>
    float ConnectionRating { get; }

    Task RefreshConnectionRatingAsync(CancellationToken cancellationToken);

    IEnumerable<IPacket> GetNextUpdateCollection();

    // bool ShouldSendMarioLocationUpdate(IPlayer remotePlayer, DateTime lastUpdateTimestamp);

    // bool ShouldSendCosutmeUpdate(IPlayer remotePlayer, string lastMarioCostumeName, string lastCappyCostumeName, DateTime lastUpdateTimestamp);

    // bool ShouldSendCappyLocationUpdate(IPlayer remotePlayer, bool lastIsThrown, DateTime lastUpdateTimestamp);

    // bool ShouldSendCaptureUpdate(IPlayer remotePlayer, string lastCaptureModelName, DateTime lastUpdateTimestamp);

    // bool ShouldSendStageUpdate(IPlayer remotePlayer, byte lastStageId, DateTime lastUpdateTimestamp);

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
