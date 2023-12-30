namespace SuperMarioOdysseyOnline.Server.World;

[AttributeUsage(AttributeTargets.Field)]
public class StageAttribute(Kingdom kingdom, bool isHomeStage = false) : Attribute
{
    public Kingdom Kingdom => kingdom;

    public bool IsHomeStage => isHomeStage;
}
