using Newtonsoft.Json;
using SoundShapesServer.Authentication;

namespace SoundShapesServer.Responses.Game.Sessions;

public class GameSessionResponse
{
    public GameSessionResponse(GameSession session)
    {
        ExpirationDate = session.ExpiresAt.ToUnixTimeMilliseconds();
        Id = session.Id ?? "";
        if (session.User != null) User = new SessionUserResponse(session.User);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("expires")] public long? ExpirationDate { get; set; }
    [JsonProperty("person")] public SessionUserResponse? User { get; set; }
}