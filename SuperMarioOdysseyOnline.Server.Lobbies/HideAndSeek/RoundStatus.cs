using System.Text.Json.Serialization;

namespace SuperMarioOdysseyOnline.Server.Lobbies.HideAndSeek;

[JsonConverter(typeof(JsonStringEnumConverter<RoundStatus>))]
public enum RoundStatus
{
    Queued,
    Loading,
    Playing,
    Paused,
    Completed
}
