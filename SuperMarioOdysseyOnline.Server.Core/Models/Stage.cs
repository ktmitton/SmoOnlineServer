namespace SuperMarioOdysseyOnline.Server.Models;

public record Stage(byte Scenario, string Name, bool Is2d)
{
    public Stage() : this(default, string.Empty, default)
    {
    }
}
