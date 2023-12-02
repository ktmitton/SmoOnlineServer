using Microsoft.AspNetCore.Connections;
using SuperMarioOdysseyOnline.Server.Events;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Tcp;

public class TcpConnectionHandler(IServiceProvider serviceProvider) : ConnectionHandler
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        InitializeConnectionContext(connection, scope.ServiceProvider);

        await InitializePlayerConnectionAsync(scope.ServiceProvider, new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);

        var eventStream = scope.ServiceProvider.GetRequiredService<IEventStream>();
        await eventStream.InitializeAsync(connection.ConnectionClosed);

        await eventStream.Task;
    }

    public void InitializeConnectionContext(ConnectionContext connectionContext, IServiceProvider provider)
    {
        var connectionAccessor = provider.GetRequiredService<ITcpConnectionContextAccessor>();
        connectionAccessor.ConnectionContext = connectionContext;
    }

    private static Task InitializePlayerConnectionAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        var playerAccessor = provider.GetRequiredService<IPlayerConnectionAccessor>();
        playerAccessor.PlayerConnection = provider.GetRequiredService<TcpPlayerConnection>();

        return playerAccessor.PlayerConnection.InitializeAsync(cancellationToken);
    }
}
