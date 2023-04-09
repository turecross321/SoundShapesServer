using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class PlayerNewsResponse
{
    [JsonProperty("items")] public string[] Items { get; set; }
}