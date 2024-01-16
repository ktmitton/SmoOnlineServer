using SuperMarioOdysseyOnline.Server.World;
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

        var playerChunks = Players.ShuffleAndChunk(SeekersPerRound, true).ToArray();

        var stagePool = new List<Stage>();
        while (stagePool.Count < playerChunks.Length)
        {
            stagePool.AddRange(Stages.Shuffle());
        }

        return new Set(playerChunks.Select((seekers, index) => new Round(stagePool[index], seekers, Players.Except(seekers))).ToList());
    }
}
