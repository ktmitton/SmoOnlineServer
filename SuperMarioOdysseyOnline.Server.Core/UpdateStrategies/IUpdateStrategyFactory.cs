using SuperMarioOdysseyOnline.Server.Core.Connections;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Core.UpdateStrategies;

public interface IUpdateStrategyFactory
{
    Task<IUpdateStrategy> CreateAsync(ILobby lobby, IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken);
}

internal class DefaultUpdateStrategyFactory : IUpdateStrategyFactory
{
    public Task<IUpdateStrategy> CreateAsync(ILobby lobby, IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken)
    {
        IUpdateStrategy strategy = new DefaultUpdateStrategy(lobby, player);

        return Task.FromResult(strategy);
    }
}
