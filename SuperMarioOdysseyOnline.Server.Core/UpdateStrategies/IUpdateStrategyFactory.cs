using Microsoft.Extensions.DependencyInjection;
using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Lobbies;

namespace SuperMarioOdysseyOnline.Server.UpdateStrategies;

public interface IUpdateStrategyFactory
{
    Task<IUpdateStrategy> CreateAsync(ILobby lobby, IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken);
}

public class DefaultUpdateStrategyFactory(IServiceProvider serviceProvider) : IUpdateStrategyFactory
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public Task<IUpdateStrategy> CreateAsync(ILobby lobby, IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken)
    {
        IUpdateStrategy strategy = new DefaultUpdateStrategy(lobby, player);

        _serviceProvider.GetKeyedService<IUpdateStrategy>(lobby.GetType());

        return Task.FromResult(strategy);
    }
}
