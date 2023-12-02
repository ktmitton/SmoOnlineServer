using SuperMarioOdysseyOnline.Server.Players;
using SuperMarioOdysseyOnline.Server.Scenarios;

namespace SuperMarioOdysseyOnline.Server.Packets;

public static class PacketHandlerServiceCollectionExtensions
{
    public static IServiceCollection AddPacketHandler(this IServiceCollection services)
        => services.AddSingleton<IPacketHandler, DefaultPacketHandler>();
}

public interface IPacketHandler
{
    void Handle(IPlayer player, Packet packet);
}

internal class DefaultPacketHandler(IPlayerManager playerManager, IScenarioManager scenarioManager) : IPacketHandler
{
    public void Handle(IPlayer player, Packet packet)
    {
        playerManager.GetPlayer(player).HandleReceivedPacket(packet);
        scenarioManager.HandleReceivedPacket(player, packet);
    }
}
