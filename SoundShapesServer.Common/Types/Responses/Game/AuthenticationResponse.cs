using Newtonsoft.Json;

namespace SoundShapesServer.Common.Types.Responses.Game;

public record AuthenticationResponse
{
    [JsonProperty("session")] public required SessionResponse Session { get; set; }
}