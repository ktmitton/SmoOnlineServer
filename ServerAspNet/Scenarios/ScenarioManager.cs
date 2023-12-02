using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Scenarios;

public interface IScenarioManager
{
    void HandleReceivedPacket(IPlayer player, Packet packet);
}

internal class ScenarioManager : IScenarioManager
{
    public void HandleReceivedPacket(IPlayer player, Packet packet)
    {
    }
}
