namespace SuperMarioOdysseyOnline.Server.Players;

public interface IPlayerConnectionAccessor
{
    IPlayerConnection? PlayerConnection { get; set; }
}

internal class DefaultPlayerConnectionAccessor : IPlayerConnectionAccessor
{
    public IPlayerConnection? PlayerConnection { get; set; }
}
