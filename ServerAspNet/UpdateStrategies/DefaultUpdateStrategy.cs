using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.UpdateStrategies;

public static class DefaultUpdateStrategyServiceExtensions
{
    public static IServiceCollection AddDefaultUpdateStrategy(this IServiceCollection serviceCollection)
        => serviceCollection.AddTransient<IUpdateStrategy, DefaultUpdateStrategy>();
}

internal class DefaultUpdateStrategy : IUpdateStrategy
{
    public TimeSpan MinimumUpdatePeriod { get; } = TimeSpan.FromMilliseconds(10);

    public float ConnectionRating => 0;

    /// <summary>
    /// The frequency with which location updates should be sent for players on different stages, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan DifferentStageLocationUpdateFrequency = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The frequency with which location updates should be sent for distant players on the same stage, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan DistantProximityLocationUpdateFrequency = TimeSpan.FromMilliseconds(100);

    /// <summary>
    /// The frequency with which cosmetic updates should be sent, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan CosmeticFrequency = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The frequency with which stage updates should be sent, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan StageUpdateFrequency = TimeSpan.FromSeconds(10);

    /// <summary>
    /// The maximum distance between players on the same stage where location details should be sent with every update.
    /// </summary>
    /// <remarks>
    /// If two players on the same stage are further apart, location details will update based on <see cref="HighPriorityFrequency"/>.
    /// If two players are on different stages, location details will update based on <see cref="LowPriorityFrequency"/>.
    /// </remarks>
    private const float MaximumImmediateLocationUpdateDistance = 10;

    public Task RefreshConnectionRatingAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public IEnumerable<Packet> GetNextUpdateCollection()
        => Enumerable.Empty<Packet>();

    // public bool ShouldSendMarioLocationUpdate(IPlayer remotePlayer, DateTime lastUpdateTimestamp)
    // {
    //     if (_connectionPlayer is null)
    //     {
    //         throw new Exception("Connected player not found, has the strategy been initialized?");
    //     }

    //     var arePlayersOnDifferentStages = _connectionPlayer.Stage.Id != remotePlayer.Stage.Id;

    //     if (arePlayersOnDifferentStages)
    //     {
    //         return lastUpdateTimestamp < DateTime.Now.Subtract(DifferentStageLocationUpdateFrequency);
    //     }

    //     var distance = Math.Abs(Vector3.Distance(_connectionPlayer.Mario.Position, remotePlayer.Mario.Position));

    //     var arePlayersFarApart = distance <= MaximumImmediateLocationUpdateDistance;

    //     if (arePlayersFarApart)
    //     {
    //         return lastUpdateTimestamp < DateTime.Now.Subtract(DistantProximityLocationUpdateFrequency);
    //     }

    //     return true;
    // }

    // public bool ShouldSendCosutmeUpdate(IPlayer remotePlayer, string lastMarioCostumeName, string lastCappyCostumeName, DateTime lastUpdateTimestamp)
    // {
    //     var hasMarioCostumeChanged = !remotePlayer.Mario.CostumeName.Equals(lastMarioCostumeName);
    //     var hasCappyCostumeChanged = !remotePlayer.Cappy.CostumeName.Equals(lastCappyCostumeName);

    //     if (hasMarioCostumeChanged || hasCappyCostumeChanged)
    //     {
    //         return true;
    //     }

    //     return lastUpdateTimestamp < DateTime.Now.Subtract(CosmeticFrequency);
    // }

    // public bool ShouldSendCappyLocationUpdate(IPlayer remotePlayer, bool lastIsThrown, DateTime lastUpdateTimestamp)
    // {
    //     return remotePlayer.Cappy.IsThrown || lastIsThrown;
    // }

    // public bool ShouldSendCaptureUpdate(IPlayer remotePlayer, string lastCaptureModelName, DateTime lastUpdateTimestamp)
    // {
    //     if (remotePlayer.Mario.CaptureModelName.Equals(lastCaptureModelName))
    //     {
    //         return lastUpdateTimestamp < DateTime.Now.Subtract(CosmeticFrequency);
    //     }

    //     return true;
    // }

    // public bool ShouldSendStageUpdate(IPlayer remotePlayer, byte lastStageId, DateTime lastUpdateTimestamp)
    // {
    //     if (remotePlayer.Stage.Id == lastStageId)
    //     {
    //         return lastUpdateTimestamp < DateTime.Now.Subtract(StageUpdateFrequency);
    //     }

    //     return true;
    // }
}
