using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Levels;

public class FeaturedResponse
{
    [JsonProperty("00_queryType")] public string queryType { get; set; }
    [JsonProperty("00_buttonLabel")] public string buttonLabel { get; set; }
    [JsonProperty("00_query")] public string query { get; set; }
    [JsonProperty("00_panelDescription")] public string panelDescription { get; set; }
    [JsonProperty("00_imageUrl")] public string imageUrl { get; set; }
    [JsonProperty("00_panelTitle")] public string panelTitle { get; set; }
}