using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;
using SuperMarioOdysseyOnline.Server.World;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

public class UpdateStrategyDecorator(IUpdateStrategy updateStrategy, Lobby lobby, IPlayer connectedPlayer) : IUpdateStrategy
{
    private readonly IUpdateStrategy _updateStrategy = updateStrategy;

    private readonly Lobby _lobby = lobby;

    private readonly IPlayer _connectedPlayer = connectedPlayer;

    public TimeSpan MinimumUpdatePeriod => _updateStrategy.MinimumUpdatePeriod;

    private readonly Dictionary<IPlayer, SeekingUpdateLog> _seekingUpdateLogs = [];

    private TransportUpdateLog _transferUpdateLog = new(DateTime.MinValue, default, default);

    private static readonly TimeSpan SeekingFrequency = TimeSpan.FromSeconds(5);

    public IEnumerable<IPacket> GetNextUpdateCollection() =>
        _updateStrategy
            .GetNextUpdateCollection()
            .Concat(GetTagUpdates())
            .Concat(GetStageUpdates());

    public IEnumerable<IPacket> GetStageUpdates()
    {
        var currentRound = _lobby.Set.CurrentRound;

        if (currentRound is not null && !string.IsNullOrWhiteSpace(_connectedPlayer.Stage.Name))
        {
            var playerStatus = currentRound.PlayerStatuses[_connectedPlayer];

            switch (currentRound.Status)
            {
                case RoundStatus.Loading:
                    if (currentRound.Id != _transferUpdateLog.RoundId)
                    {
                        var stage = playerStatus.IsSeeking ? Stage.HomeShipInsideStage : currentRound.Stage.Stage;

                        _transferUpdateLog = new TransportUpdateLog(DateTime.Now, currentRound.Id, stage);

                        yield return new ChangeStagePacket(_connectedPlayer, stage);
                    }

                    break;
                case RoundStatus.Playing:
                    var hasRoundStarted = currentRound.PlayTime >= TimeSpan.Zero;
                    var isInOdyssey = _transferUpdateLog.Stage == Stage.HomeShipInsideStage;

                    if (playerStatus.IsSeeking && hasRoundStarted && isInOdyssey)
                    {
                        _transferUpdateLog = new TransportUpdateLog(DateTime.Now, currentRound.Id, currentRound.Stage.Stage);

                        yield return new ChangeStagePacket(_connectedPlayer, currentRound.Stage.Stage);
                    }

                    break;
            }
        }
    }

    private IEnumerable<IPacket> GetTagUpdates()
    {
        if (_lobby.Set.CurrentRound is not null)
        {
            foreach (var playerStatus in _lobby.Set.CurrentRound.PlayerStatuses)
            {
                var player = playerStatus.Key;
                var isSeeking = playerStatus.Value.IsSeeking;

                if (!_seekingUpdateLogs.TryGetValue(player, out var log))
                {
                    log = new SeekingUpdateLog(DateTime.MinValue, false);
                }

                if (ShouldSendTagUpdate(isSeeking, log))
                {
                    yield return new TagPacket(player, playerStatus.Value.IsSeeking);

                    _seekingUpdateLogs[player] = new SeekingUpdateLog(DateTime.Now, playerStatus.Value.IsSeeking);
                }
            }
        }
    }

    private static bool ShouldSendTagUpdate(bool isSeeking, SeekingUpdateLog lastLog)
    {
        if (isSeeking != lastLog.IsSeeking)
        {
            return true;
        }

        return lastLog.Timestamp < DateTime.Now.Subtract(SeekingFrequency);
    }

    public Task RefreshConnectionRatingAsync(CancellationToken cancellationToken)
        => _updateStrategy.RefreshConnectionRatingAsync(cancellationToken);

    private record PlayerUpdateLog(SeekingUpdateLog SeekingUpdateLog, TransportUpdateLog TransportUpdateLog);

    private record SeekingUpdateLog(DateTime Timestamp, bool IsSeeking);

    private record TransportUpdateLog(DateTime Timestamp, Guid? RoundId, Stage? Stage);
}
