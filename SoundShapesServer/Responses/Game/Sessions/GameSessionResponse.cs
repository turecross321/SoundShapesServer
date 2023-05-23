using Newtonsoft.Json;
using SoundShapesServer.Types.Sessions;

namespace SoundShapesServer.Responses.Game.Sessions;

public class GameSessionResponse
{
    public GameSessionResponse(GameSession session)
    {
        // Game doesn't actually listen to expiry date, until it disconnects, and it will only try to get a new session
        // if the old one is already expired. This can only lead to problems, hence this always being set to 0
        ExpirationDate = 0; 
        Id = session.Id;
        User = new SessionUserResponse(session.User);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("expires")] public long? ExpirationDate { get; set; }
    [JsonProperty("person")] public SessionUserResponse? User { get; set; }
}