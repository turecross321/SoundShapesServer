using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Sessions;

public class GameSessionResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("expires")] public long ExpirationDate { get; set; }
    [JsonProperty("person")] public SessionUserResponse User { get; set; }
    [JsonProperty("service")] public Service? Service { get; set; }
}