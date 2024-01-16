using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace SuperMarioOdysseyOnline.Server.Lobbies;

public class LobbyConfiguration
{
    public List<LobbyDetails> Lobbies { get; set; } = [];
}

public record LobbyDetails(string Name, string Url, string Type)
{
    public Guid Id { get; } = Guid.NewGuid();

    public EndPoint? EndPoint
    {
        get
        {
            var pattern = @"^([^:]+)://([^:]+):([0-9]+)$";
            var parts = Regex.Match(Url, pattern, RegexOptions.IgnoreCase);

            if (parts.Success)
            {
                var hostname = parts.Groups[2].Value.ToLower();
                var port = int.Parse(parts.Groups[3].Value);

                return new IPEndPoint(
                    hostname switch
                    {
                        "localhost" => IPAddress.Loopback,
                        "*" => IPAddress.Any,
                        _ => IPAddress.Parse(hostname),
                    },
                    port
                );
            }
            return default;
        }
    }
}
