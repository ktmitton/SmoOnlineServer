using Bedrock.Framework;
using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using SuperMarioOdysseyOnline.Server.Players;

namespace SuperMarioOdysseyOnline.Server.Tcp;

public static class TcpHostServiceServiceCollectionExtentions
{
    public static IServiceCollection AddTcpConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TcpConfiguration>(configuration.GetSection("TcpConfiguration"));

        return services
            .AddHostedService<TcpHostedService>()
            .AddScoped<ITcpConnectionContextAccessor, DefaultTcpConnectionContextAccessor>()
            .AddScoped<TcpPlayerConnection>();
    }
}

public class TcpPortConfiguration
{
    public int Port { get; set; }
}

public class TcpConfiguration
{
    public List<TcpPortConfiguration> PortConfigurations { get; set; } = [];
}

public class TcpHostedService(IServiceProvider serviceProvider, IOptions<TcpConfiguration> configuration) : IHostedService
{
    private readonly Bedrock.Framework.Server _server = new ServerBuilder(serviceProvider)
            .UseSockets(sockets =>
            {
                foreach (var socketConfiguration in configuration.Value.PortConfigurations)
                {
                    sockets.ListenLocalhost(socketConfiguration.Port, builder => builder.UseConnectionLogging().UseConnectionHandler<TcpConnectionHandler>());
                }
            })
            .Build();

    public Task StartAsync(CancellationToken cancellationToken)
        => _server.StartAsync(cancellationToken);

    public Task StopAsync(CancellationToken cancellationToken)
        => _server.StopAsync(cancellationToken);
}
