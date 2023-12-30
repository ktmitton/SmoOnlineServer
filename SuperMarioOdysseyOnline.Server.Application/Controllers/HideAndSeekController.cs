using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SuperMarioOdysseyOnline.Server.Core.World;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;
using static SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek.Lobby;

namespace SuperMarioOdysseyOnline.Server.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HideAndSeekController(ILobbyCollection lobbies) : ControllerBase
{
    private static readonly Kingdom[] _defaultBlacklistedKingdoms =
    [
        Kingdom.DarkSide,
        Kingdom.DarkerSide,
        Kingdom.Odyssey
    ];

    private static readonly Stage[] _defaultStagePool =
        Enum.GetValues<Stage>()
            .Where(x =>
                x.GetType()
                    .GetMember(x.ToString())
                    .First()
                    .GetCustomAttributes<StageAttribute>()
                    .Any(x => x.IsHomeStage && !_defaultBlacklistedKingdoms.Contains(x.Kingdom))
            )
            .ToArray();

    private readonly ILobbyCollection _lobbies = lobbies;

    [HttpGet("{LobbyId}")]
    public RefreshDetails GetLobbyDetails(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            return hideAndSeek.GetRefreshDetails();
        }

        throw new ArgumentException($"Lobby id [{LobbyId}] was not a valid Hide and Seek lobby.");
    }

    [HttpPost("{LobbyId}/createnewset/{SeekersPerRound}")]
    public void CreateNewSet(Guid LobbyId, int SeekersPerRound)
    {
        var defaultBlacklistedKingdoms = new Kingdom[]
        {
            Kingdom.DarkSide,
            Kingdom.DarkerSide,
            Kingdom.Odyssey
        };

        var memberInfo = Enum.GetValues<Stage>().Where(x =>
            x.GetType()
                .GetMember(x.ToString())
                .First()
                .GetCustomAttributes<StageAttribute>()
                .Any(x => x.IsHomeStage && !defaultBlacklistedKingdoms.Contains(x.Kingdom))
        );

        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.InitializeNewSet(_defaultStagePool, SeekersPerRound);
        }

    }

    [HttpPost("{LobbyId}/extendcurrentset/{SeekersPerRound}")]
    public void ExtendCurrentSet(Guid LobbyId, int SeekersPerRound)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.ExtendCurrentSet(_defaultStagePool, SeekersPerRound);
        }
    }

    [HttpPost("{LobbyId}/play")]
    public void Play(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Set.CurrentRound?.Play();
        }
    }

    [HttpPost("{LobbyId}/pause")]
    public void Pause(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Set.CurrentRound?.Pause();
        }
    }

    [HttpPost("{LobbyId}/load")]
    public void Load(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Set.CurrentRound?.Load();
        }
    }

    [HttpPost("{LobbyId}/lock")]
    public void Lock(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Lock();
        }
    }

    [HttpPost("{LobbyId}/unlock")]
    public void Unlock(Guid LobbyId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Unlock();
        }
    }

    [HttpPost("{LobbyId}/tag/{PlayerId}")]
    public void TagPlayer(Guid LobbyId, Guid PlayerId)
    {
        if (!_lobbies.TryGetLobby(LobbyId, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is Lobby hideAndSeek)
        {
            hideAndSeek.Set.CurrentRound?.TagPlayer(PlayerId);
        }
    }
}
