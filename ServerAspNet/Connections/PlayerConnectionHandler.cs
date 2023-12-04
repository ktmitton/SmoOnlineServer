using Microsoft.AspNetCore.Connections;
using SuperMarioOdysseyOnline.Server.Events;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Connections;

public class PlayerConnectionHandler(IServiceProvider serviceProvider, ILogger<PlayerConnectionHandler> logger) : ConnectionHandler
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly ILogger _logger = logger;

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        _logger.LogInformation(
            "[{ConnectionId}] Connection Started: Local [{LocalEndpoint}] Remote [{RemoteEndpoint}]",
            connection.ConnectionId,
            connection.LocalEndPoint?.ToString(),
            connection.RemoteEndPoint?.ToString()
        );

        await using var scope = _serviceProvider.CreateAsyncScope();

        var initializedCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
            connection.ConnectionClosed,
            new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token
        ).Token;

        InitializeConnectionContext(connection, scope.ServiceProvider);

        await InitializePlayerConnectionAsync(scope.ServiceProvider, initializedCancellationToken);

        await scope.ServiceProvider.GetRequiredService<IEventStream>().Task;

        _logger.LogInformation(
            "[{ConnectionId}] Connection Ended: Local [{LocalEndpoint}] Remote [{RemoteEndpoint}]",
            connection.ConnectionId,
            connection.LocalEndPoint?.ToString(),
            connection.RemoteEndPoint?.ToString()
        );
    }

    private static void InitializeConnectionContext(ConnectionContext connectionContext, IServiceProvider provider)
    {
        var connectionAccessor = provider.GetRequiredService<IConnectionContextAccessor>();
        connectionAccessor.ConnectionContext = connectionContext;
    }

    private static Task InitializePlayerConnectionAsync(IServiceProvider provider, CancellationToken cancellationToken)
    {
        var playerAccessor = provider.GetRequiredService<IPlayerConnectionAccessor>();
        playerAccessor.PlayerConnection = provider.GetRequiredService<TcpPlayerConnection>();

        return playerAccessor.PlayerConnection.InitializeAsync(cancellationToken);
    }
}
