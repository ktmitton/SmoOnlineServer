namespace SuperMarioOdysseyOnline.Server.Models;

public record CapturedEntity(string Name)
{
    public CapturedEntity() : this(string.Empty)
    {
    }
}
