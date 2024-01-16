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

    public static IEnumerable<IEnumerable<T>> ShuffleAndChunk<T>(this IEnumerable<T> source, int chunkSize, bool fillFinalChunk)
    {
        var chunks = source.Shuffle().Chunk(chunkSize);

        foreach (var chunk in chunks)
        {
            if (!fillFinalChunk || (chunk.Length == chunkSize))
            {
                yield return chunk;
            }
            else
            {
                var fillers = source.Except(chunk).Shuffle().Take(chunkSize - chunk.Length).ToArray();

                yield return chunk.Concat(fillers);
            }
        }
    }
}
