using System.Collections.Concurrent;

namespace SuperMarioOdysseyOnline.Server.Players;

public interface IPlayerManager
{
    IPlayer GetPlayer(Guid id);

    IPlayer GetPlayer(Guid id, string name);

    virtual IPlayer GetPlayer(IPlayer player) => GetPlayer(player.Id);

    IEnumerable<IPlayer> GetOtherPlayers(Guid playerId);
}

public class PlayerManager : IPlayerManager
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = new ();

    public IPlayer GetPlayer(Guid id)
        => _players.GetOrAdd(id, (x) => new Player(x));

    public IPlayer GetPlayer(Guid id, string name)
    {
        var player = GetPlayer(id);

        player.Name = name;

        return player;
    }

    public IEnumerable<IPlayer> GetOtherPlayers(Guid playerId)
        => _players.Where((pair) => pair.Key != playerId).Select((pair) => pair.Value);
}
