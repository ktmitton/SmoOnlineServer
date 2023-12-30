using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SuperMarioOdysseyOnline.Server.Lobbies;

namespace SuperMarioOdysseyOnline.Server.Core.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapLobbies(this WebApplication app)
    {
        var lobbies =
            from implementation in Assembly.GetCallingAssembly().GetTypes()
            let attributes = implementation.GetCustomAttributes<LobbyAttribute>(true)
            where attributes is not null && attributes.Any()
            from attribute in attributes
            select new { Implementation = implementation, attribute.Type };

        foreach(var lobby in lobbies)
        {
            app.Services.GetRequiredService<ILobbyFactory>().MapLobby(lobby.Type, lobby.Implementation);
        }

        return app;
    }
}
