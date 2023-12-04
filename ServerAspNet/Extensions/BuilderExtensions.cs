using Bedrock.Framework;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Events;
using SuperMarioOdysseyOnline.Server.Lobby;
using SuperMarioOdysseyOnline.Server.Packets;
using SuperMarioOdysseyOnline.Server.Players;
using SuperMarioOdysseyOnline.Server.Scenarios;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddSuperMarioOdysseyOnline(this WebApplicationBuilder builder)
    {
        builder.Services.AddConnectionContextAccessor();

        builder.Services.AddDefaultUpdateStrategy();
        builder.Services.AddPlayers();
        builder.Services.AddPacketHandler();
        builder.Services.AddEventStream();
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IScenarioManager, ScenarioManager>();
        builder.Services.AddTcpPlayerConnection();

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
