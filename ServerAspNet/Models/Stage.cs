namespace SuperMarioOdysseyOnline.Server.Models;

public record Stage(byte Id, string Name, bool Is2d)
{
    public Stage() : this(default, string.Empty, default)
    {
    }
}
