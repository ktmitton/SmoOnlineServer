using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

namespace SuperMarioOdysseyOnline.Server.Core.Lobby;

public interface ILobby
{
    Guid Id { get; }

    string Name { get; }

    IEnumerable<IPlayer> Players { get; }

    IPlayer GetOrAddPlayer(Guid id);

    IPlayer GetOrAddPlayer(ConnectPacket packet)
        => GetOrAddPlayer(packet.Id, packet.Data.ClientName);

    IPlayer GetOrAddPlayer(Guid id, string name)
    {
        var player = GetOrAddPlayer(id);

        player.Name = name;

        return player;
    }

    IPlayer GetOrAddPlayer(IPlayer player) => GetOrAddPlayer(player.Id, player.Name);

    IEnumerable<IPlayer> GetOpponents(IPlayer player) => Players.Where(x => x.Id != player.Id);

    void HandleReceivedPacket(IPlayer player, IPacket packet);

    IEnumerable<IPacket> GetNextUpdateCollection(IPlayer player) => Enumerable.Empty<IPacket>();
}
