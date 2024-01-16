using System.Numerics;
using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Models;

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

    Task RefreshConnectionRatingAsync(CancellationToken cancellationToken);

    IEnumerable<IPacket> GetNextUpdateCollection();
}


public class DefaultUpdateStrategy(ILobby lobby, IPlayer player) : IUpdateStrategy
{
    private readonly ILobby _lobby = lobby;

    private readonly IPlayer _connectedPlayer = player;

    public TimeSpan MinimumUpdatePeriod { get; } = TimeSpan.FromMilliseconds(10);

    /// <summary>
    /// The frequency with which location updates should be sent for players on different stages, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan DifferentStageLocationUpdateFrequency = TimeSpan.FromSeconds(1);

    private static readonly TimeSpan MarioUnchangedUpdateFrequency = TimeSpan.FromSeconds(1);

    /// <summary>
    /// The frequency with which cosmetic updates should be sent, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan CosmeticFrequency = TimeSpan.FromSeconds(30);

    /// <summary>
    /// The frequency with which stage updates should be sent, regardless of last update values.
    /// </summary>
    private static readonly TimeSpan StageUpdateFrequency = TimeSpan.FromSeconds(10);

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
        => _lobby.Players.Except([_connectedPlayer]).SelectMany(GetRemotePlayerUpdates);

    private IEnumerable<IPacket> GetRemotePlayerUpdates(IPlayer remotePlayer)
    {
        if (!_playerUpdateLogs.TryGetValue(remotePlayer.Id, out var logs))
        {
            yield return new ConnectPacket(remotePlayer);

            logs = PlayerUpdateLog.CreateDefault();
        }

        if (ShouldSendCosutmeUpdate(remotePlayer, logs.Costume))
        {
            yield return new CostumePacket(remotePlayer);

            logs = logs with { Costume = new PlayerCostumeLog(remotePlayer) };
        }

        if (ShouldSendStageUpdate(remotePlayer, logs.Stage))
        {
            yield return new PlayerStagePacket(remotePlayer);

            logs = logs with { Stage = new PlayerStageLog(remotePlayer) };
        }

        if (ShouldSendRemotePlayerLocationUpdate(remotePlayer, logs.Mario))
        {
            yield return new MarioRenderPacket(remotePlayer);

            logs = logs with { Mario = new PlayerMarioLog(remotePlayer) };
        }

        if (ShouldSendCappyLocationUpdate(remotePlayer, logs.Cappy))
        {
            yield return new CappyRenderPacket(remotePlayer);

            logs = logs with { Cappy = new PlayerCappyLog(remotePlayer) };
        }

        if (ShouldSendCaptureUpdate(remotePlayer, logs.Capture))
        {
            yield return new CapturePacket(remotePlayer);

            logs = logs with { Capture = new PlayerCaptureLog(remotePlayer) };
        }

        _playerUpdateLogs[remotePlayer.Id] = logs;
    }

    private bool ShouldSendRemotePlayerLocationUpdate(IPlayer remotePlayer, PlayerMarioLog lastLog)
    {
        var hasMarioDataChanged = remotePlayer.Mario != lastLog.Mario;
        if (!hasMarioDataChanged)
        {
            return lastLog.Timestamp < DateTime.Now.Subtract(MarioUnchangedUpdateFrequency);
        }

        var arePlayersOnDifferentStages = _connectedPlayer.Stage.Name != remotePlayer.Stage.Name;
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
        return remotePlayer.Cappy != lastLog.Cappy;
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
        return (remotePlayer.Stage.Scenario != lastLog.Scenario) || !remotePlayer.Stage.Name.Equals(lastLog.Name);
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
                new PlayerMarioLog(),
                new PlayerCostumeLog(DateTime.MinValue, string.Empty, string.Empty),
                new PlayerCappyLog(DateTime.MinValue, new()),
                new PlayerCaptureLog(DateTime.MinValue, string.Empty),
                new PlayerStageLog(DateTime.MinValue, string.Empty, default)
            );
    }

    private record PlayerMarioLog(DateTime Timestamp, Mario Mario)
    {
        public PlayerMarioLog()
            : this(DateTime.MinValue, new())
        {
        }

        public PlayerMarioLog(IPlayer player)
            : this(DateTime.Now, player.Mario)
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

    private record PlayerCappyLog(DateTime Timestamp, Cappy Cappy)
    {
        public PlayerCappyLog(IPlayer player)
            : this(DateTime.Now, player.Cappy)
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
