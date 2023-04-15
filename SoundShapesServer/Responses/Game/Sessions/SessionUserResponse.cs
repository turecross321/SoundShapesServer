using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Sessions;

public class SessionUserResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("display_name")] public string Username { get; set; }
}