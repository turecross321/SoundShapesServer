using Newtonsoft.Json;

namespace SoundShapesServer.Types.Responses.Game;

public record AuthenticationResponse
{
    [JsonProperty("session")] public required SessionResponse Session { get; set; }
}