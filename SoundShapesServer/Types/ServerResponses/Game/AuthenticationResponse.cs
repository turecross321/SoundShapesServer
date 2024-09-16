using Newtonsoft.Json;

namespace SoundShapesServer.Types.ServerResponses.Game;

public record AuthenticationResponse
{
    [JsonProperty("session")] public required SessionResponse Session { get; set; }
}