using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using Microsoft.Extensions.Options;

namespace SuperMarioOdysseyOnline.Server.Core.Lobby;

public interface ILobbyCollection : IEnumerable<ILobby>
{
    bool TryGetLobby(EndPoint endpoint, [NotNullWhen(true)] out ILobby? lobby);

    bool TryGetLobby(Guid id, [NotNullWhen(true)] out ILobby? lobby);
}

internal class DefaultLobbyCollection(ILobbyFactory lobbyFactory, IOptions<LobbyConfiguration> configuration) : ILobbyCollection
{
    private readonly Dictionary<LobbyDetails, ILobby> _lobbies = new(
        configuration.Value.Lobbies.Select(
            details => new KeyValuePair<LobbyDetails, ILobby>(details, lobbyFactory.Create(details))
        )
    );

    public IEnumerator<ILobby> GetEnumerator()
        => _lobbies.Select(pair => pair.Value).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => _lobbies.Select(pair => pair.Value).GetEnumerator();

    public bool TryGetLobby(EndPoint endpoint, [NotNullWhen(true)] out ILobby? lobby)
    {
        lobby = _lobbies.Where(pair => pair.Key.EndPoint?.Equals(endpoint) ?? false).FirstOrDefault().Value;

        return lobby is not null;
    }

    public bool TryGetLobby(Guid id, [NotNullWhen(true)] out ILobby? lobby)
    {
        throw new NotImplementedException();
    }
}
