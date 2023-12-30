using SuperMarioOdysseyOnline.Server.Core.World;
using SuperMarioOdysseyOnline.Server.Lobbies.Extensions;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

internal class SetBuilder
{
    public IEnumerable<IPlayer> Players { get; set; } = [];

    public IEnumerable<Stage> Stages { get; set; } = [];

    public int SeekersPerRound { get; set; } = 2;

    public Set Build()
    {
        if (SeekersPerRound >= Players.Count())
        {
            throw new InvalidOperationException($"There are not enough players [{Players.Count()}] for the desired round size [{SeekersPerRound}].");
        }

        var playerPool = Players.Shuffle().Chunk(SeekersPerRound).ToList();
        var lastPlayerChunk = playerPool.Last();

        if (lastPlayerChunk.Length < SeekersPerRound)
        {
            var secondaryPool = Players.Except(lastPlayerChunk).Shuffle().Take(SeekersPerRound - lastPlayerChunk.Length);

            foreach (var secondaryPlayer in secondaryPool)
            {
                lastPlayerChunk.Append(secondaryPlayer);
            }
        }

        var stagePool = new List<Stage>();
        while (stagePool.Count < playerPool.Count)
        {
            stagePool.AddRange(Stages.Shuffle());
        }

        return new Set(playerPool.Select((seekers, index) => new Round(stagePool[index], seekers, Players.Except(seekers))).ToList());
    }
}
