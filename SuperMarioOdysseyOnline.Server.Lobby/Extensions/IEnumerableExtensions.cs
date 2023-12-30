namespace SuperMarioOdysseyOnline.Server.Lobbies.Extensions;

internal static class IEnumerableExtensions
{
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        var randomGenerator = new Random();

        var buffer = source.ToList();

        for (var i = 0; i < buffer.Count; ++i)
        {
            var nextIndex = randomGenerator.Next(i, buffer.Count);

            yield return buffer[nextIndex];

            buffer[nextIndex] = buffer[i];
        }
    }
}
