namespace SuperMarioOdysseyOnline.Server.Events;

public static class EventStreamServiceCollectionExtensions
{
    public static IServiceCollection AddEventStream(this IServiceCollection serviceCollection)
        => serviceCollection.AddTransient<IEventStream, DefaultEventStream>();
}
