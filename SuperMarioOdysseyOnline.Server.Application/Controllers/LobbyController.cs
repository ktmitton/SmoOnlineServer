using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using SuperMarioOdysseyOnline.Server.Core.Lobby;
using SuperMarioOdysseyOnline.Server.Lobby;

namespace SuperMarioOdysseyOnline.Server.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LobbyController(ILobbyCollection lobbies) : ControllerBase
{
    private readonly ILobbyCollection _lobbies = lobbies;

    [HttpGet("all")]
    public IEnumerable<Details> GetAllLobbies()
    {
        return _lobbies.Select(x =>
        {
            switch (x)
            {
                case HideAndSeekLobby:
                    return new Details(x.Id, x.Name, LobbyType.HideAndSeek);
                case CoopLobby:
                    return new Details(x.Id, x.Name, LobbyType.Coop);
                default:
                    throw new Exception("Unknown lobby type");
            }
        });
    }

    public record Details(Guid Id, string Name, LobbyType LobbyType);

    [JsonConverter(typeof(JsonStringEnumConverter<LobbyType>))]
    public enum LobbyType
    {
        HideAndSeek,
        Coop
    }
}
