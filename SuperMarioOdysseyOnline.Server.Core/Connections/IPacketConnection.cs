using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;

namespace SuperMarioOdysseyOnline.Server.Core.Connections;

public interface IPacketConnection
{
    Task<T> ReceiveNextPacketAsync<T>(CancellationToken cancellationToken) where T : IPacket;

    Task<IPacket> ReceiveNextPacketAsync(CancellationToken cancellationToken);

    Task SendPacketAsync(IPacket packet, CancellationToken cancellationToken);
}
