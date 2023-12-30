using Microsoft.AspNetCore.Builder;
using SuperMarioOdysseyOnline.Server.Core.Extensions;

namespace SuperMarioOdysseyOnline.Server.Lobbies.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication MapDefaultLobbies(this WebApplication app)
        => app.MapLobbies();
}
