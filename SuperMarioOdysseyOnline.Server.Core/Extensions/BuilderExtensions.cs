using Bedrock.Framework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SuperMarioOdysseyOnline.Server.Core.Connections;
using SuperMarioOdysseyOnline.Server.Lobbies;

namespace SuperMarioOdysseyOnline.Server.Core.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSuperMarioOdysseyOnline(this WebApplicationBuilder builder)
    {
        builder.Services.AddDefaultUpdateStrategyFactory();
        builder.Services.AddDefaultLobbyCollection();
        builder.Services.AddDefaultTcpPacketMessageHandlers();

        builder.Services.AddHostedService<ServerHostedService>();
        builder.Services.AddOptions<ServerHostedServiceOptions>()
            .Configure<IServiceProvider, IOptions<LobbyConfiguration>>((options, serviceProvider, LobbyConfiguration) =>
            {
                options.ServerBuilder = new ServerBuilder(serviceProvider)
                    .UseSockets(sockets =>
                    {
                        foreach (var lobby in LobbyConfiguration.Value.Lobbies)
                        {
                            sockets.Listen(
                                lobby.EndPoint,
                                builder => builder.UseConnectionLogging().UseConnectionHandler<PlayerConnectionHandler>()
                            );
                        }
                    });
            });

        builder.AddDefaultLobbyConfiguration();

        return builder;
    }
}
