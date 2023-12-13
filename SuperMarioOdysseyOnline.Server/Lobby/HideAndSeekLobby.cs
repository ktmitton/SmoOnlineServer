using System.Collections.Concurrent;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Lobby;

[Lobby("HideAndSeek")]
internal class HideAndSeekLobby(Guid id, string name) : ILobby
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = [];

    private readonly ConcurrentDictionary<Guid, bool> _seekerStatus = [];

    private readonly ConcurrentDictionary<Guid, TimeSpan> _hiddenTimespans = [];

    public Guid Id => id;

    public string Name => name;

    public HideAndSeekLobby(LobbyDetails lobbyDetails)
        : this(Guid.NewGuid(), lobbyDetails.Name)
    {
    }

    public IEnumerable<IPlayer> Players => _players.Select(pair => pair.Value);

    public IPlayer GetOrAddPlayer(Guid id) => _players.GetOrAdd(id, (x) => new Player(x));

    public void HandleReceivedPacket(IPlayer player, IPacket packet)
    {
        switch (packet)
        {
            case TagPacket tagPacket:
                if ((tagPacket.Data.UpdateType & TagFlags.State) != 0)
                {
                    _seekerStatus[player.Id] = tagPacket.Data.IsIt;
                }

                if ((tagPacket.Data.UpdateType & TagFlags.Time) != 0)
                {
                    _hiddenTimespans[player.Id] = tagPacket.Data.TimeSpan;
                }

                break;
        }
    }
}
