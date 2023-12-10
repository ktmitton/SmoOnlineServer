namespace SuperMarioOdysseyOnline.Server.Lobby;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class LobbyAttribute(string type) : Attribute
{
    public string Type => type;
}
