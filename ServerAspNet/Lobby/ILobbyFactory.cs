
namespace SuperMarioOdysseyOnline.Server.Lobby;

public interface ILobbyFactory
{
    ILobby Create(LobbyDetails lobbyDetails);
}

internal class DefaultLobbyFactory : ILobbyFactory
{
    public ILobby Create(LobbyDetails lobbyDetails)
        => new HideAndSeekLobby(lobbyDetails);
}
