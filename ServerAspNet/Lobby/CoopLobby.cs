using System.Collections.Concurrent;
using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.Lobby;

[Lobby("Coop")]
internal class CoopLobby(Guid id, string name) : ILobby
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = new();

    public Guid Id => id;

    public string Name => name;

    public CoopLobby(LobbyDetails lobbyDetails)
        : this(Guid.NewGuid(), lobbyDetails.Name)
    {
    }

    public IEnumerable<IPlayer> Players => _players.Select(pair => pair.Value);

    public IPlayer GetOrAddPlayer(Guid id) => _players.GetOrAdd(id, (x) => new Player(x));

    public void HandleReceivedPacket(IPacket packet)
    {
    }
}
