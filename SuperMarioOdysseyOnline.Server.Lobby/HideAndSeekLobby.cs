using System.Collections.Concurrent;
using System.Text.Json.Serialization;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Lobby;

[Lobby("HideAndSeek")]
public class HideAndSeekLobby(Guid id, string name) : ILobby
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = [];

    private readonly ConcurrentDictionary<Guid, bool> _seekerStatus = [];

    private readonly ConcurrentDictionary<Guid, TimeSpan> _hiddenTimespans = [];

    private readonly object _loadLock = new();

    public List<Round> Set { get; private set; } = [];

    public Guid Id => id;

    public string Name => name;

    public bool IsLocked { get; private set; }

    public Round? CurrentRound { get; set; }

    public IEnumerable<IPlayer> Players => _players.Select(pair => pair.Value);

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

    public HideAndSeekLobby(LobbyDetails lobbyDetails)
        : this(Guid.NewGuid(), lobbyDetails.Name)
    {
        for (var i = 0; i < 10; ++i)
        {
            GetOrAddPlayer(Guid.NewGuid()).Name  = $"Player {i}";
        }
    }

    public void HandleReceivedPacket(IPlayer player, IPacket packet)
    {
        switch (packet)
        {
            case TagPacket tagPacket:
                if ((tagPacket.Data.UpdateType & TagFlags.State) != 0)
                {
                    if (_seekerStatus.TryAdd(player.Id, true) && CurrentRound is not null)
                    {
                        _hiddenTimespans.AddOrUpdate(
                            player.Id,
                            CurrentRound.PlayTime,
                            (id, hiddenTime) => hiddenTime.Add(CurrentRound.PlayTime)
                        );
                    }
                }

                if ((tagPacket.Data.UpdateType & TagFlags.Time) != 0)
                {
                    _hiddenTimespans[player.Id] = tagPacket.Data.TimeSpan;
                }

                if (!Players.Any(x => !_seekerStatus.ContainsKey(x.Id)))
                {
                    CurrentRound?.Complete();
                }

                break;
        }
    }

    public RefreshDetails GetRefreshDetails()
    {
        return new RefreshDetails(
            IsLocked: IsLocked,
            Players: Players.Select(x => {
                _seekerStatus.TryGetValue(x.Id, out var isIt);
                var hiddenTimespan = _hiddenTimespans.TryGetValue(x.Id, out var timeSpan) ? timeSpan : TimeSpan.Zero;
                if (!isIt && CurrentRound is not null && CurrentRound.PlayTime > TimeSpan.Zero)
                {
                    hiddenTimespan = hiddenTimespan.Add(CurrentRound.PlayTime);
                }

                return new RefreshPlayerDetails(
                    x.Id,
                    x.Name,
                    isIt,
                    hiddenTimespan
                );
            }),
            Set: Set,
            CurrentRound: CurrentRound
        );
    }

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock()
    {
        IsLocked = false;
    }

    private List<Round> BuildSet(int seekersPerRound)
    {
        var playerPool = _players.Select(pair => pair.Value).ToList();

        if (seekersPerRound >= playerPool.Count)
        {
            throw new InvalidOperationException($"There are not enough players [{playerPool.Count}] for the desired round size [{seekersPerRound}].");
        }

        var randomGenerator = new Random();

        var set = new List<Round>();

        while (playerPool.Count != 0)
        {
            var roundPlayers = new List<IPlayer>();

            var tempPlayerPool = playerPool;
            if (seekersPerRound > playerPool.Count)
            {
                roundPlayers.AddRange(playerPool);
                tempPlayerPool = _players.Select(pair => pair.Value).Where(x => !playerPool.Contains(x)).ToList();
                playerPool = [];
            }

            while (roundPlayers.Count < seekersPerRound)
            {
                var nextPlayerIndex = randomGenerator.Next(tempPlayerPool.Count);
                roundPlayers.Add(tempPlayerPool[nextPlayerIndex]);
                tempPlayerPool.RemoveAt(nextPlayerIndex);
            }

            set.Add(new Round("", roundPlayers));
        }

        return set;
    }

    public void CreateNewSet(int seekersPerRound)
    {
        Set = BuildSet(seekersPerRound);
        CurrentRound = Set.First();
    }

    public void ExtendCurrentSet(int seekersPerRound)
    {
        Set.AddRange(BuildSet(seekersPerRound));
    }

    public void LoadCurrentRound()
    {
        lock (_loadLock)
        {
            _seekerStatus.Clear();

            if (CurrentRound is not null)
            {
                CurrentRound.Load();

                foreach (var seeker in CurrentRound.Seekers)
                {
                    _seekerStatus[seeker.Id] = true;
                }
            }
        }
    }

    public void Play() => CurrentRound?.Play();

    public void Pause() => CurrentRound?.Pause();

    public record RefreshDetails(bool IsLocked, IEnumerable<RefreshPlayerDetails> Players, IEnumerable<Round> Set, Round? CurrentRound);

    public record RefreshPlayerDetails(Guid Id, string Name, bool IsSeeking, TimeSpan TotalTimeHidden);

    public class Round(string stageName, IEnumerable<IPlayer> players)
    {
        public Guid Id { get; } = Guid.NewGuid();

        public string StageName => stageName;

        public IEnumerable<IPlayer> Seekers => players;

        private TimeSpan _accumulatedPlayTime = TimeSpan.Zero;

        public TimeSpan PlayTime => _lastStartTime.HasValue
            ? _accumulatedPlayTime.Add(DateTime.Now.Subtract(_lastStartTime.Value))
            : _accumulatedPlayTime;

        public RoundStatus Status { get; private set; } = RoundStatus.Queued;

        private DateTime? _lastStartTime = default;

        private readonly object _statusLock = new();

        private void UpdateStatus(RoundStatus status)
        {
            lock (_statusLock)
            {
                if (status == RoundStatus.Playing)
                {
                    _lastStartTime = DateTime.Now;
                }
                else if (status ==  RoundStatus.Loading)
                {
                    _accumulatedPlayTime = TimeSpan.FromSeconds(-60);
                }
                else if (_lastStartTime.HasValue)
                {
                    _accumulatedPlayTime = _accumulatedPlayTime.Add(DateTime.Now.Subtract(_lastStartTime.Value));
                    _lastStartTime = default;
                }

                Status = status;
            }
        }

        public void Load() => UpdateStatus(RoundStatus.Loading);

        public void Complete() => UpdateStatus(RoundStatus.Completed);

        public void Play() => UpdateStatus(RoundStatus.Playing);

        public void Pause() => UpdateStatus(RoundStatus.Paused);
    }

    [JsonConverter(typeof(JsonStringEnumConverter<RoundStatus>))]
    public enum RoundStatus
    {
        Queued,
        Loading,
        Playing,
        Paused,
        Completed
    }
}
