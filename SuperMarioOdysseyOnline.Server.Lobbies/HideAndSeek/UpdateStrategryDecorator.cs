using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

public class UpdateStrategyDecorator(IUpdateStrategy updateStrategy, Lobby lobby, IPlayer player) : IUpdateStrategy
{
    private readonly IUpdateStrategy _updateStrategy = updateStrategy;

    private readonly Lobby _lobby = lobby;

    private readonly IPlayer _connectedPlayer = player;

    public TimeSpan MinimumUpdatePeriod => _updateStrategy.MinimumUpdatePeriod;

    private readonly Dictionary<Guid, PlayerUpdateLog> _playerUpdateLogs = [];

    public IEnumerable<IPacket> GetNextUpdateCollection()
    {
        var basePackets = _updateStrategy.GetNextUpdateCollection();

        var seekers = _lobby.Seekers;

        return basePackets;
    }

    public Task RefreshConnectionRatingAsync(CancellationToken cancellationToken)
        => _updateStrategy.RefreshConnectionRatingAsync(cancellationToken);

    private record PlayerUpdateLog(DateTime Timestamp, bool IsIt);
}
