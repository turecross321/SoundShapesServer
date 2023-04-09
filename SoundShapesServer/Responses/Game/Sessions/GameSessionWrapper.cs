using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Sessions;

public class GameSessionWrapper
{
    [JsonProperty("session")] public GameSessionResponse Session { get; set; }
}