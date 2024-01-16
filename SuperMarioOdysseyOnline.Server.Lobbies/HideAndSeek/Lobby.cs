using System.Collections.Concurrent;
using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;
using SuperMarioOdysseyOnline.Server.World;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

[Lobby("HideAndSeek")]
public class Lobby(Guid id, string name) : ILobby
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = [];

    public readonly ConcurrentDictionary<Guid, bool> _seekerStatus = [];

    private readonly ConcurrentDictionary<Guid, TimeSpan> _hiddenTimespans = [];

    public Set Set { get; private set; } = new();

    public Guid Id => id;

    public string Name => name;

    public bool IsLocked { get; private set; }

    public IEnumerable<IPlayer> Players => _players.Select(pair => pair.Value);

    public IEnumerable<IPlayer> Seekers => Players.Where(x => _seekerStatus.TryGetValue(x.Id, out var isIt) && isIt);

    public IPlayer GetOrAddPlayer(Guid id)
    {
        if (!IsLocked)
        {
            return _players.GetOrAdd(id, (x) => new Player(x));
        }
        else if (_players.TryGetValue(id, out var player))
        {
            return player;
        }

        throw new InvalidOperationException($"Player id [{id}] is not already a member of a locked lobby and cannot be added");
    }

    public Lobby(LobbyDetails lobbyDetails)
        : this(Guid.NewGuid(), lobbyDetails.Name)
    {
        // GetOrAddPlayer(Guid.NewGuid()).Name  = $"Ken";
        // GetOrAddPlayer(Guid.NewGuid()).Name  = $"Patrick";
        // GetOrAddPlayer(Guid.NewGuid()).Name  = $"Chris";
        // GetOrAddPlayer(Guid.NewGuid()).Name  = $"Michael";
        // GetOrAddPlayer(Guid.NewGuid()).Name  = $"Mikey";

        GetOrAddPlayer(Guid.NewGuid()).Name  = $"Hot Apple Toddy";
        GetOrAddPlayer(Guid.NewGuid()).Name  = $"The Little Merman";
        GetOrAddPlayer(Guid.NewGuid()).Name  = $"Solo Caravan";
        GetOrAddPlayer(Guid.NewGuid()).Name  = $"The Smartest Idiot";
    }

    public void HandleReceivedPacket(IPlayer player, IPacket packet)
    {
        switch (packet)
        {
            case TagPacket tagPacket:
                if ((tagPacket.Data.UpdateType & TagFlags.State) != 0 && tagPacket.Data.IsIt)
                {
                    Set.CurrentRound?.TagPlayer(player);
                }

                if ((tagPacket.Data.UpdateType & TagFlags.Time) != 0)
                {
                    _hiddenTimespans[player.Id] = tagPacket.Data.TimeSpan;
                }

                break;
        }
    }

    public RefreshDetails GetRefreshDetails()
        => new(
            IsLocked: IsLocked,
            Players: Players.Select(player => {
                var isSeeking = Set.CurrentRound?.PlayerStatuses[player]?.IsSeeking ?? false;
                var hiddenTime = Set.Rounds
                    .Select(x => x.PlayerStatuses.TryGetValue(player, out var value) ? value.TimeHidden : TimeSpan.Zero)
                    .Aggregate(TimeSpan.Zero, (accumulator, currentValue) => accumulator.Add(currentValue));

                return new PlayerDetails(player, isSeeking, hiddenTime);
            }),
            Set: Set.Rounds,
            CurrentRound: Set?.CurrentRound
        );

    public void Lock() => IsLocked = true;

    public void Unlock() => IsLocked = false;

    public void InitializeNewSet(IEnumerable<Stage> stagePool, int seekersPerRound)
        => Set = new SetBuilder()
        {
            Players = Players,
            Stages = stagePool,
            SeekersPerRound = seekersPerRound
        }.Build();

    public void ExtendCurrentSet(IEnumerable<Stage> stagePool, int seekersPerRound)
        => Set.Extend(new SetBuilder()
        {
            Players = Players,
            Stages = stagePool,
            SeekersPerRound = seekersPerRound
        }.Build());

    public Task<IUpdateStrategy> CreateUpdateStrategyAsync(IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken)
    {
        var baseStrategy = new DefaultUpdateStrategy(this, player);

        IUpdateStrategy updateStrategy = new UpdateStrategyDecorator(baseStrategy, this, player);

        return Task.FromResult(updateStrategy);
    }

    public record RefreshDetails(bool IsLocked, IEnumerable<PlayerDetails> Players, IEnumerable<Round> Set, Round? CurrentRound);
}
