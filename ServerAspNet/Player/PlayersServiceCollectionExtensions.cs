namespace SuperMarioOdysseyOnline.Server.Players;

public static class PlayerServiceCollectionExtensions
{
    public static IServiceCollection AddPlayers(this IServiceCollection serviceCollection)
        => serviceCollection
            .AddScoped<IPlayerConnectionAccessor, DefaultPlayerConnectionAccessor>()
            .AddSingleton<IPlayerManager, PlayerManager>();
}
