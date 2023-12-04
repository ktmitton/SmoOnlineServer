using Microsoft.AspNetCore.Connections;

namespace SuperMarioOdysseyOnline.Server.Connections;

public static class TcpHostServiceServiceCollectionExtentions
{
    public static IServiceCollection AddConnectionContextAccessor(this IServiceCollection services)
        => services.AddScoped<IConnectionContextAccessor, DefaultConnectionContextAccessor>();
}

public interface IConnectionContextAccessor
{
    ConnectionContext? ConnectionContext { get; set; }
}

internal class DefaultConnectionContextAccessor : IConnectionContextAccessor
{
    public ConnectionContext? ConnectionContext { get; set; }
}
