
namespace SuperMarioOdysseyOnline.Server.Lobby;

public interface ILobbyFactory
{
    void RegisterLobby<T>(string lobbyType) where T : ILobby;

    void RegisterLobby(string lobbyType, Type implementation);

    ILobby Create(LobbyDetails lobbyDetails);
}

internal delegate ILobby CreateLobbyDelegate(LobbyDetails lobbyDetails);

internal class DefaultLobbyFactory(IServiceProvider serviceProvider) : ILobbyFactory
{
    private readonly Dictionary<string, CreateLobbyDelegate> _registrations = [];

    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void RegisterLobby<T>(string lobbyType) where T : ILobby
    {
        _registrations[lobbyType] = (lobbyDetails) => ActivatorUtilities.CreateInstance<T>(_serviceProvider, lobbyDetails);
    }

    public void RegisterLobby(string lobbyType, Type implementation)
    {
        if (!implementation.IsAssignableTo(typeof(ILobby)))
        {
            throw new ArgumentException($"Implemenation [{implementation.FullName}] is not assignable to ILobby.", nameof(implementation));
        }

        _registrations[lobbyType] = (lobbyDetails) => (ILobby)ActivatorUtilities.CreateInstance(_serviceProvider, implementation, lobbyDetails);
    }

    public ILobby Create(LobbyDetails lobbyDetails)
    {
        if (!_registrations.TryGetValue(lobbyDetails.Type, out var lobbyDelegate))
        {
            throw new InvalidOperationException($"No registration found for lobby type [{lobbyDetails.Type}].");
        }

        return lobbyDelegate.Invoke(lobbyDetails);
    }
}
