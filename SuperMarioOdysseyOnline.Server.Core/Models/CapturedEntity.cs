namespace SuperMarioOdysseyOnline.Server.Core.Models;

public record CapturedEntity(string Name)
{
    public CapturedEntity() : this(string.Empty)
    {
    }
}
