using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json.Serialization;
using SuperMarioOdysseyOnline.Server.World;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

public class Round(Stage stage, IEnumerable<IPlayer> initialSeekers, IEnumerable<IPlayer> initialHiders)
{
    public Guid Id { get; } = Guid.NewGuid();

    public StageDetails Stage { get; } = StageDetails.FromStage(stage);

    private readonly ImmutableArray<IPlayer> _initialSeekers = initialSeekers.ToImmutableArray();

    private readonly ImmutableArray<IPlayer> _initialHiders = initialHiders.ToImmutableArray();

    private readonly ConcurrentDictionary<IPlayer, TimeSpan> _currentSeekers = new(
        initialSeekers.Select(x => new KeyValuePair<IPlayer, TimeSpan>(x, TimeSpan.Zero))
    );

    public IEnumerable<PlayerDetails> InitialSeekers => _initialSeekers.Select(x => new PlayerDetails(x, true, TimeSpan.Zero));

    [JsonIgnore]
    public Dictionary<IPlayer, PlayerDetails> PlayerStatuses => new(
        _initialSeekers.Concat(_initialHiders).Select(player => {
            var isSeeking = _currentSeekers.TryGetValue(player, out var time);

            if (!isSeeking)
            {
                time = PlayTime > TimeSpan.Zero ? PlayTime : TimeSpan.Zero;
            }

            return new KeyValuePair<IPlayer, PlayerDetails>(player, new PlayerDetails(player, isSeeking, time));
        })
    );

    private TimeSpan _accumulatedPlayTime = TimeSpan.Zero;

    public TimeSpan PlayTime => _lastStartTime.HasValue
        ? _accumulatedPlayTime.Add(DateTime.Now.Subtract(_lastStartTime.Value))
        : _accumulatedPlayTime;

    public RoundStatus Status { get; private set; } = RoundStatus.Queued;

    private DateTime? _lastStartTime = default;

    public void TagPlayer(IPlayer player)
    {
        var hiddenTime = PlayTime > TimeSpan.Zero ? PlayTime : TimeSpan.Zero;
        var wasTagSuccessful = _initialHiders.Contains(player) && _currentSeekers.TryAdd(player, hiddenTime);
        var areAllPlayersTagged = (_initialSeekers.Length + _initialHiders.Length) == _currentSeekers.Count;

        if (wasTagSuccessful && areAllPlayersTagged)
        {
            UpdateStatus(RoundStatus.Completed);
        }
    }

    public void TagPlayer(Guid playerId)
    {
        var hider = _initialHiders.FirstOrDefault(x => x.Id == playerId);

        if (hider is not null)
        {
            TagPlayer(hider);
        }
    }

    private readonly object _statusLock = new();

    private static bool CanStatusChange(RoundStatus originalStatus, RoundStatus newStatus)
    {
        var validNewStatuses = Enumerable.Empty<RoundStatus>();

        switch (originalStatus)
        {
            case RoundStatus.Queued:
                validNewStatuses = new[] { RoundStatus.Loading, RoundStatus.Playing, RoundStatus.Completed };

                break;
            case RoundStatus.Loading:
                validNewStatuses = new[] { RoundStatus.Playing, RoundStatus.Completed };

                break;
            case RoundStatus.Playing:
                validNewStatuses = new[] { RoundStatus.Paused, RoundStatus.Completed };

                break;
            case RoundStatus.Paused:
                validNewStatuses = new [] { RoundStatus.Playing, RoundStatus.Completed };

                break;
        }

        return validNewStatuses.Contains(newStatus);
    }

    private void UpdateStatus(RoundStatus newStatus)
    {
        lock (_statusLock)
        {
            if (!CanStatusChange(Status, newStatus))
            {
                return;
            }

            Status = newStatus;

            switch (Status)
            {
                case RoundStatus.Loading:
                    _lastStartTime = default;
                    _accumulatedPlayTime = TimeSpan.FromSeconds(-60);

                    break;
                case RoundStatus.Playing:
                    _lastStartTime = DateTime.Now;

                    break;
                case RoundStatus.Paused:
                case RoundStatus.Completed:
                    if (_lastStartTime is not null)
                    {
                        _accumulatedPlayTime = _accumulatedPlayTime.Add(DateTime.Now.Subtract(_lastStartTime.Value));
                        _lastStartTime = default;
                    }

                    break;
            }
        }
    }

    public void Load() => UpdateStatus(RoundStatus.Loading);

    public void Play() => UpdateStatus(RoundStatus.Playing);

    public void Pause() => UpdateStatus(RoundStatus.Paused);
}
