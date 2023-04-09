using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Sessions;

public class GameSessionWrapper
{
    [JsonProperty("session")] public GameSessionResponse Session { get; set; }
}