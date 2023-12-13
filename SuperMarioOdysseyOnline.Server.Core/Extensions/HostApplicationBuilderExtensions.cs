using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperMarioOdysseyOnline.Server.Core.Lobby;

namespace SuperMarioOdysseyOnline.Server.Core.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddDefaultLobbyConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<LobbyConfiguration>(builder.Configuration.GetSection("Lobby"));

        return builder;
    }
}
