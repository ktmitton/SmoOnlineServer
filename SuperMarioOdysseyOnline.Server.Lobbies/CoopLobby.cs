using System.Collections.Concurrent;
using SuperMarioOdysseyOnline.Server.Connections;
using SuperMarioOdysseyOnline.Server.Connections.Packets;
using SuperMarioOdysseyOnline.Server.UpdateStrategies;

namespace SuperMarioOdysseyOnline.Server.Lobbies;

[Lobby("Coop")]
public class CoopLobby(Guid id, string name) : ILobby
{
    private readonly ConcurrentDictionary<Guid, IPlayer> _players = [];

    private readonly ConcurrentQueue<int> _shineSyncQueue = [];

    private readonly ConcurrentDictionary<int, bool> _shines = [];

    private readonly ConcurrentDictionary<Guid, int?> _playerSyncData = [];

    public Guid Id => id;

    public string Name => name;

    public CoopLobby(LobbyDetails lobbyDetails)
        : this(Guid.NewGuid(), lobbyDetails.Name)
    {
    }

    public IEnumerable<IPlayer> Players => _players.Select(pair => pair.Value);

    public IPlayer GetOrAddPlayer(Guid id) => _players.GetOrAdd(id, (x) => new Player(x));

    public void HandleReceivedPacket(IPlayer player, IPacket packet)
    {
        switch (packet)
        {
            case PlayerStagePacket playerStagePacket:
                switch (playerStagePacket.Data.Stage.Name)
                {
                    case "CapWorldHomeStage" when playerStagePacket.Data.Stage.Scenario == 0:
                        _playerSyncData[player.Id] = default;

                        break;
                    case "WaterfallWorldHomeStage" when !_playerSyncData[player.Id].HasValue:
                        _playerSyncData.TryAdd(player.Id, 0);

                        break;
                }

                break;
            case ShinePacket shinePacket:
                if (_playerSyncData[player.Id].HasValue && _shines.TryAdd(shinePacket.Data.ShineId, true))
                {
                    _shineSyncQueue.Enqueue(shinePacket.Data.ShineId);
                }

                break;
        }
    }

    public IEnumerable<IPacket> GetNextUpdateCollection(IPlayer player)
    {
        var updates = new List<IPacket>();

        if (
            _playerSyncData.TryGetValue(player.Id, out var lastSyncIndex) &&
            lastSyncIndex.HasValue &&
            (lastSyncIndex < _shineSyncQueue.Count)
        )
        {
            updates.AddRange(_shineSyncQueue.Skip(lastSyncIndex.Value).Select(x => new ShinePacket(x)));

            _playerSyncData.TryUpdate(player.Id, lastSyncIndex.Value + updates.Count, lastSyncIndex.Value);
        }

        return updates;
    }

    public Task<IUpdateStrategy> CreateUpdateStrategyAsync(IPlayer player, IPacketConnection packetConnection, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
