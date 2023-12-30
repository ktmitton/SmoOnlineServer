namespace SuperMarioOdysseyOnline.Server.Models;

public record Mario(Location Location, Costume Costume, Animation Animation, CapturedEntity? CapturedEntity)
{
    public Mario() : this(new Location(), new Costume(), new Animation(), default)
    {
    }
}
