using Microsoft.AspNetCore.Mvc;
using SuperMarioOdysseyOnline.Server.Core.Lobby;
using SuperMarioOdysseyOnline.Server.Lobby;
using static SuperMarioOdysseyOnline.Server.Lobby.HideAndSeekLobby;

namespace SuperMarioOdysseyOnline.Server.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HideAndSeekController(ILobbyCollection lobbies) : ControllerBase
{
    private readonly ILobbyCollection _lobbies = lobbies;

    [HttpGet("{id}")]
    public RefreshDetails GetLobbyDetails(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            return hideAndSeek.GetRefreshDetails();
        }

        throw new ArgumentException($"Lobby id [{id}] was not a valid Hide and Seek lobby.");
    }

    [HttpPost("{id}/createnewset/{seekersPerRound}")]
    public void CreateNewSet(Guid id, int seekersPerRound)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.CreateNewSet(seekersPerRound);
        }
    }

    [HttpPost("{id}/extendcurrentset/{seekersPerRound}")]
    public void ExtendCurrentSet(Guid id, int seekersPerRound)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.ExtendCurrentSet(seekersPerRound);
        }
    }

    [HttpPost("{id}/play")]
    public void Play(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.CurrentRound?.Play();
        }
    }

    [HttpPost("{id}/pause")]
    public void Pause(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.CurrentRound?.Pause();
        }
    }

    [HttpPost("{id}/load")]
    public void Load(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.LoadCurrentRound();
        }
    }

    [HttpPost("{id}/lock")]
    public void Lock(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.Lock();
        }
    }

    [HttpPost("{id}/unlock")]
    public void Unlock(Guid id)
    {
        if (!_lobbies.TryGetLobby(id, out var lobby))
        {
            throw new Exception("lobby not found");
        }

        if (lobby is HideAndSeekLobby hideAndSeek)
        {
            hideAndSeek.Unlock();
        }
    }
}
