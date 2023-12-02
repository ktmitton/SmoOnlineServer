using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.UpdateStrategies;

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

    IEnumerable<Packet> GetNextUpdateCollection();

    // bool ShouldSendMarioLocationUpdate(IPlayer remotePlayer, DateTime lastUpdateTimestamp);

    // bool ShouldSendCosutmeUpdate(IPlayer remotePlayer, string lastMarioCostumeName, string lastCappyCostumeName, DateTime lastUpdateTimestamp);

    // bool ShouldSendCappyLocationUpdate(IPlayer remotePlayer, bool lastIsThrown, DateTime lastUpdateTimestamp);

    // bool ShouldSendCaptureUpdate(IPlayer remotePlayer, string lastCaptureModelName, DateTime lastUpdateTimestamp);

    // bool ShouldSendStageUpdate(IPlayer remotePlayer, byte lastStageId, DateTime lastUpdateTimestamp);
}
