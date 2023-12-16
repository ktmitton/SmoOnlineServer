using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SuperMarioOdysseyOnline.Server.Core.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Core.Connections.Tcp;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Core.Connections;

public class PlayerConnectionHandler(IServiceProvider serviceProvider, ILogger<PlayerConnectionHandler> logger) : ConnectionHandler
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly ILogger _logger = logger;

    public override async Task OnConnectedAsync(ConnectionContext context)
    {
        _logger.LogInformation(
            "[{ConnectionId}] Connection Started: Local [{LocalEndpoint}] Remote [{RemoteEndpoint}]",
            context.ConnectionId,
            context.LocalEndPoint?.ToString(),
            context.RemoteEndPoint?.ToString()
        );

        if (context.LocalEndPoint is not null)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var lobbyCollection = scope.ServiceProvider.GetRequiredService<ILobbyCollection>();

            if (!lobbyCollection.TryGetLobby(context.LocalEndPoint, out var lobby))
            {
                _logger.LogWarning(
                    "[{ConnectionId}] Connection Aborted: No lobby is associated with Endpoint [{LocalEndpoint}]",
                    context.ConnectionId,
                    context.LocalEndPoint?.ToString()
                );

                return;
            }


            var connection = ActivatorUtilities.CreateInstance<TcpPacketConnection>(scope.ServiceProvider, context);

            var initializeCancellationToken = CancellationTokenSource.CreateLinkedTokenSource(
                    context.ConnectionClosed,
                    new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token
                ).Token;
            var connectPacket = await connection.ReceiveNextPacketAsync<ConnectPacket>(initializeCancellationToken);
            await connection.SendPacketAsync(new InitPacket(8), initializeCancellationToken);

            var player = lobby.GetOrAddPlayer(connectPacket.Id);

            var eventStream = ActivatorUtilities.CreateInstance<EventStream>(
                scope.ServiceProvider,
                lobby,
                player,
                connection
            );

            await eventStream.Task;
        }

        _logger.LogInformation(
            "[{ConnectionId}] Connection Ended: Local [{LocalEndpoint}] Remote [{RemoteEndpoint}]",
            context.ConnectionId,
            context.LocalEndPoint?.ToString(),
            context.RemoteEndPoint?.ToString()
        );
    }
}
