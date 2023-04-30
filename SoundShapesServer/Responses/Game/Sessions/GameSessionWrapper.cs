using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Sessions;

public class GameSessionWrapper
{
    public GameSessionWrapper(GameSessionResponse gameSessionResponse)
    {
        GameSession = gameSessionResponse;
    }

    [JsonProperty("session")] public GameSessionResponse GameSession { get; set; }
}