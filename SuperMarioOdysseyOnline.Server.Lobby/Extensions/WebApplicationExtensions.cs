using Microsoft.AspNetCore.Builder;
using SuperMarioOdysseyOnline.Server.Core.Extensions;

namespace SuperMarioOdysseyOnline.Server.Lobby.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapDefaultLobbies(this WebApplication app)
        => app.MapLobbies();
}
