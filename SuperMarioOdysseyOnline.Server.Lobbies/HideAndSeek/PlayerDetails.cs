namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

public record PlayerDetails(Guid Id, string Name, bool IsSeeking, TimeSpan TimeHidden)
{
    public PlayerDetails(IPlayer player, bool isSeeking, TimeSpan timeHidden)
        : this(player.Id,  player.Name, isSeeking, timeHidden)
    {
    }
}
