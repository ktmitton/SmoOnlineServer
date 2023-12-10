using SuperMarioOdysseyOnline.Server.Packets;

namespace SuperMarioOdysseyOnline.Server.Lobby;

public interface ILobby
{
    Guid Id { get; }

    string Name { get; }

    IEnumerable<IPlayer> Players { get; }

    IPlayer GetOrAddPlayer(Guid id);

    virtual IPlayer GetOrAddPlayer(ConnectPacket packet)
        => GetOrAddPlayer(packet.Id, packet.Data.ClientName);

    virtual IPlayer GetOrAddPlayer(Guid id, string name)
    {
        var player = GetOrAddPlayer(id);

        player.Name = name;

        return player;
    }

    virtual IPlayer GetOrAddPlayer(IPlayer player) => GetOrAddPlayer(player.Id, player.Name);

    virtual IEnumerable<IPlayer> GetOpponents(IPlayer player) => Players.Where(x => x.Id != player.Id);

    void HandleReceivedPacket(IPacket packet);
}
