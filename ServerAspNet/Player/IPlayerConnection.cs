using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Players;

public interface IPlayerConnection
{
    IPlayer? Player { get; }

    IUpdateStrategy? PreferredUpdateStrategy { get; }

    IEnumerable<IUpdateStrategy> AvailableUpdateStrategies { get; }

    Task InitializeAsync(CancellationToken cancellationToken);

    Task<Packet> ReceiveNextPacketAsync(CancellationToken cancellationToken);

    Task SendPacketAsync(Packet packet, CancellationToken cancellationToken);
}
