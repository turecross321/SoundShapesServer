using Newtonsoft.Json;

namespace SoundShapesServer.Responses.RecentActivity;

public class NewsResponse
{
    [JsonProperty("00_title")] public string Title { get; set; }
    [JsonProperty("00_text")] public string Summary { get; set; }
    [JsonProperty("00_fullText")] public string FullText { get; set; }
    [JsonProperty("00_url")] public string Url { get; set; }

    // [JsonProperty("00_image")] public string image { get; set; }   // Commented out because this, for some reason, only works on RPCS3
    // [JsonProperty("00_image_full")] public string image { get; set; } // Will make the game try to get an image from "<ip>/"
}