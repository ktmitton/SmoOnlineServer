namespace SuperMarioOdysseyOnline.Server.Models;

public record Cappy(Location Location, Costume Costume, Animation Animation, bool IsThrown)
{
    public Cappy() : this(new Location(), new Costume(), new Animation(), default)
    {
    }
}
