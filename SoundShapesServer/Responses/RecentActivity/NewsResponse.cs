using Newtonsoft.Json;

namespace SoundShapesServer.Responses.RecentActivity;

public class NewsResponse
{
    [JsonProperty("00_title")] public string title { get; set; }
    [JsonProperty("00_text")] public string text { get; set; }
    [JsonProperty("00_fullText")] public string fullText { get; set; }
    [JsonProperty("00_url")] public string url { get; set; }

    // [JsonProperty("00_image")] public string image { get; set; }   // Commented out because this, for some reason, only works on RPCS3
    // [JsonProperty("00_image_full")] public string image { get; set; } // Will make the game try to get an image from "<ip>/"
}