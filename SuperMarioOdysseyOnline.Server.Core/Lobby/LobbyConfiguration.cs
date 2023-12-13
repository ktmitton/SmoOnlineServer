using System.Net;

namespace SuperMarioOdysseyOnline.Server.Core.Lobby;

public class LobbyConfiguration
{
    public List<LobbyDetails> Lobbies { get; set; } = [];
}

public record LobbyDetails(string Name, Uri Url, string Type)
{
    public Guid Id { get; } = Guid.NewGuid();

    public EndPoint? EndPoint
        => new IPEndPoint(
            Url.Host.ToLower() switch
            {
                "localhost" => IPAddress.Loopback,
                "*" => IPAddress.Any,
                _ => IPAddress.Parse(Url.Host),
            },
            Url.Port
        );
}
