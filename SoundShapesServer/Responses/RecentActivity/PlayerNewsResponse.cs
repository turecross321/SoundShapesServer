using Newtonsoft.Json;

namespace SoundShapesServer.Responses.RecentActivity;

public class PlayerNewsResponse
{
    [JsonProperty("items")] public string[] Items { get; set; }
}