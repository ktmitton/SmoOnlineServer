using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Models;

namespace SuperMarioOdysseyOnline.Server.Lobbies;

public interface IPlayer
{
    bool Connected { get; }

    bool Ignored { get; }

    string Name { get; set; }

    Guid Id { get; }

    Mario Mario { get; }

    Cappy Cappy { get; }

    Stage Stage { get; }

    void HandleReceivedPacket(IPacket packet);
}
