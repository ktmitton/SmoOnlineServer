using System.Numerics;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Core.UpdateStrategies;

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

    Task RefreshConnectionRatingAsync(CancellationToken cancellationToken);

    IEnumerable<IPacket> GetNextUpdateCollection();
}


internal class DefaultUpdateStrategy(ILobby lobby, IPlayer player) : IUpdateStrategy
{
    private readonly ILobby _lobby = lobby;

    private readonly IPlayer _connectedPlayer = player;

    public TimeSpan MinimumUpdatePeriod { get; } = TimeSpan.FromMilliseconds(10);

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

    private static readonly DistanceUpdateFrequency[] DistanceCutoffs =
    [
        new DistanceUpdateFrequency(54000f, TimeSpan.FromSeconds(1)),
        new DistanceUpdateFrequency(36000f, TimeSpan.FromMilliseconds(500)),
        new DistanceUpdateFrequency(18000f, TimeSpan.FromMilliseconds(100))
    ];

    private readonly Dictionary<Guid, PlayerUpdateLog> _playerUpdateLogs = [];

    public Task RefreshConnectionRatingAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    public IEnumerable<IPacket> GetNextUpdateCollection()
    {
        var playerUpdates = _lobby.Players.Where(x => x.Id != _connectedPlayer.Id).SelectMany(GetRemotePlayerUpdates);
        var lobbyUpdates = _lobby.GetNextUpdateCollection(_connectedPlayer);

        return [ ..playerUpdates, ..lobbyUpdates ];
    }

    public List<IPacket> GetRemotePlayerUpdates(IPlayer remotePlayer)
    {
        var updates = new List<IPacket>();

        _playerUpdateLogs.TryGetValue(remotePlayer.Id, out var logs);

        if (logs is null)
        {
            updates.Add(new ConnectPacket(remotePlayer));

            logs = PlayerUpdateLog.CreateDefault();
        }

        if (ShouldSendRemotePlayerLocationUpdate(remotePlayer, logs.Mario))
        {
            updates.Add(new MarioRenderPacket(remotePlayer));
            logs = logs with { Mario = new PlayerMarioLog() };
        }

        if (ShouldSendCosutmeUpdate(remotePlayer, logs.Costume))
        {
            updates.Add(new CostumePacket(remotePlayer));
            logs = logs with { Costume = new PlayerCostumeLog(remotePlayer) };
        }

        if (ShouldSendCappyLocationUpdate(remotePlayer, logs.Cappy))
        {
            updates.Add(new CappyRenderPacket(remotePlayer));
            logs = logs with { Cappy = new PlayerCappyLog(remotePlayer) };
        }

        if (ShouldSendCaptureUpdate(remotePlayer, logs.Capture))
        {
            updates.Add(new CapturePacket(remotePlayer));
            logs = logs with { Capture = new PlayerCaptureLog(remotePlayer) };
        }

        if (ShouldSendStageUpdate(remotePlayer, logs.Stage))
        {
            updates.Add(new PlayerStagePacket(remotePlayer));
            logs = logs with { Stage = new PlayerStageLog(remotePlayer) };
        }

        _playerUpdateLogs[remotePlayer.Id] = logs;

        return updates;
    }

    private bool ShouldSendRemotePlayerLocationUpdate(IPlayer remotePlayer, PlayerMarioLog lastLog)
    {
        var arePlayersOnDifferentStages = _connectedPlayer.Stage.Scenario != remotePlayer.Stage.Scenario;

        if (arePlayersOnDifferentStages)
        {
            return lastLog.Timestamp < DateTime.Now.Subtract(DifferentStageLocationUpdateFrequency);
        }

        var distance = Math.Abs(Vector3.Distance(_connectedPlayer.Mario.Location.Position, remotePlayer.Mario.Location.Position));

        foreach(var cutoff in DistanceCutoffs)
        {
            if (distance > cutoff.Distance)
            {
                return lastLog.Timestamp < DateTime.Now.Subtract(cutoff.Frequency);
            }
        }

        return true;
    }

    private static bool ShouldSendCosutmeUpdate(IPlayer remotePlayer, PlayerCostumeLog lastLog)
    {
        var hasMarioCostumeChanged = !remotePlayer.Mario.Costume.Name.Equals(lastLog.MarioCostumeName);
        var hasCappyCostumeChanged = !remotePlayer.Cappy.Costume.Name.Equals(lastLog.CappyCostumeName);

        if (hasMarioCostumeChanged || hasCappyCostumeChanged)
        {
            return true;
        }

        return lastLog.Timestamp < DateTime.Now.Subtract(CosmeticFrequency);
    }

    private static bool ShouldSendCappyLocationUpdate(IPlayer remotePlayer, PlayerCappyLog lastLog)
    {
        return remotePlayer.Cappy.IsThrown || lastLog.IsThrown;
    }

    private static bool ShouldSendCaptureUpdate(IPlayer remotePlayer, PlayerCaptureLog lastLog)
    {
        if (remotePlayer.Mario.CapturedEntity is null)
        {
            return lastLog.ModelName is null;
        }

        if (remotePlayer.Mario.CapturedEntity.Name.Equals(lastLog.ModelName))
        {
            return lastLog.Timestamp < DateTime.Now.Subtract(CosmeticFrequency);
        }

        return true;
    }

    private static bool ShouldSendStageUpdate(IPlayer remotePlayer, PlayerStageLog lastLog)
    {
        if ((remotePlayer.Stage.Scenario == lastLog.Scenario) || !remotePlayer.Stage.Name.Equals(lastLog.Name))
        {
            return lastLog.Timestamp < DateTime.Now.Subtract(StageUpdateFrequency);
        }

        return true;
    }

    private record PlayerUpdateLog(
        PlayerMarioLog Mario,
        PlayerCostumeLog Costume,
        PlayerCappyLog Cappy,
        PlayerCaptureLog Capture,
        PlayerStageLog Stage
    )
    {
        public static PlayerUpdateLog CreateDefault()
            => new(
                new PlayerMarioLog(DateTime.MinValue),
                new PlayerCostumeLog(DateTime.MinValue, string.Empty, string.Empty),
                new PlayerCappyLog(DateTime.MinValue, default),
                new PlayerCaptureLog(DateTime.MinValue, string.Empty),
                new PlayerStageLog(DateTime.MinValue, string.Empty, default)
            );
    }

    private record PlayerMarioLog(DateTime Timestamp)
    {
        public PlayerMarioLog()
            : this(DateTime.Now)
        {
        }
    }

    private record PlayerCostumeLog(DateTime Timestamp, string MarioCostumeName, string CappyCostumeName)
    {
        public PlayerCostumeLog(IPlayer player)
            : this(DateTime.Now, player.Mario.Costume.Name, player.Cappy.Costume.Name)
        {
        }
    }

    private record PlayerCappyLog(DateTime Timestamp, bool IsThrown)
    {
        public PlayerCappyLog(IPlayer player)
            : this(DateTime.Now, player.Cappy.IsThrown)
        {
        }
    }

    private record PlayerCaptureLog(DateTime Timestamp, string? ModelName)
    {
        public PlayerCaptureLog(IPlayer player)
            : this(DateTime.Now, player.Mario.CapturedEntity?.Name)
        {
        }
    }

    private record PlayerStageLog(DateTime Timestamp, string Name, byte Scenario)
    {
        public PlayerStageLog(IPlayer player)
            : this(DateTime.Now, player.Stage.Name, player.Stage.Scenario)
        {
        }
    }

    private record DistanceUpdateFrequency(float Distance, TimeSpan Frequency);
}
