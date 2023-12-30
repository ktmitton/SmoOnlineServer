namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

public class Set(List<Round> rounds)
{
    public Set() : this([])
    {
    }

    public List<Round> Rounds { get; } = rounds;

    public Round? CurrentRound => Rounds.FirstOrDefault(x => x.Status != RoundStatus.Completed);

    public void Extend(Set set) => Rounds.AddRange(set.Rounds);
}
