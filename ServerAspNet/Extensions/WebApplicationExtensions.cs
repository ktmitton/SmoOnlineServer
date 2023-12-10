using System.Reflection;
using SuperMarioOdysseyOnline.Server.Lobby;

namespace SuperMarioOdysseyOnline.Server.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication RegisterLobbies(this WebApplication app)
    {
        var lobbies =
            from implementation in Assembly.GetCallingAssembly().GetTypes()
            let attributes = implementation.GetCustomAttributes<LobbyAttribute>(true)
            where attributes is not null && attributes.Any()
            from attribute in attributes
            select new { Implementation = implementation, attribute.Type };

        foreach(var lobby in lobbies)
        {
            app.Services.GetRequiredService<ILobbyFactory>().RegisterLobby(lobby.Type, lobby.Implementation);
        }

        return app;
    }
}
