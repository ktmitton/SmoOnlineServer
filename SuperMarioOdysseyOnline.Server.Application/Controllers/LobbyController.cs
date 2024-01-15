using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SuperMarioOdysseyOnline.Server.Lobbies;
using SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

namespace SuperMarioOdysseyOnline.Server.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LobbyController(ILobbyCollection lobbies) : ControllerBase
{
    private readonly ILobbyCollection _lobbies = lobbies;

    [HttpGet("all")]
    public IEnumerable<Details> GetAllLobbies()
    {
        return _lobbies.Select(x => x switch
            {
                Lobby => new Details(x.Id, x.Name, LobbyType.HideAndSeek),
                CoopLobby => new Details(x.Id, x.Name, LobbyType.Coop),
                _ => throw new Exception("Unknown lobby type"),
            }
        );
    }

    public record Details(Guid Id, string Name, LobbyType Type);

    [JsonConverter(typeof(JsonStringEnumConverter<LobbyType>))]
    public enum LobbyType
    {
        HideAndSeek,
        Coop
    }
}
