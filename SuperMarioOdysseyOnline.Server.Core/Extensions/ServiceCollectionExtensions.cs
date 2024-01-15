using Bedrock.Framework.Protocols;
using Microsoft.Extensions.DependencyInjection;
using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.Connections.Tcp;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDefaultLobbyCollection(this IServiceCollection services)
        => services
            .AddSingleton<ILobbyCollection, DefaultLobbyCollection>()
            .AddSingleton<ILobbyFactory, DefaultLobbyFactory>();

    public static IServiceCollection AddDefaultTcpPacketMessageReader(this IServiceCollection services)
        => services.AddSingleton<IMessageReader<IPacket?>, PacketMessageReader>();

    public static IServiceCollection AddDefaultTcpPacketMessageWriter(this IServiceCollection services)
        => services.AddSingleton<IMessageWriter<IPacket>, PacketMessageWriter>();

    public static IServiceCollection AddDefaultTcpPacketMessageHandlers(this IServiceCollection services)
        => services.AddDefaultTcpPacketMessageReader().AddDefaultTcpPacketMessageWriter();
}
