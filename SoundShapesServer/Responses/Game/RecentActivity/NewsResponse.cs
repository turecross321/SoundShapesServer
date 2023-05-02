using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class NewsResponse
{
    public NewsResponse(NewsEntry entry)
    {
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = entry.Url;
    }

    public NewsResponse()
    {
        Title = "News";
        Summary = "There are no news yet.";
        FullText = "";
        Url = "";
    }

    [JsonProperty("00_title", NullValueHandling = NullValueHandling.Ignore)] public string Title { get; set; }
    [JsonProperty("00_text", NullValueHandling = NullValueHandling.Ignore)] public string Summary { get; set; }
    [JsonProperty("00_fullText", NullValueHandling = NullValueHandling.Ignore)] public string FullText { get; set; }
    [JsonProperty("00_url", NullValueHandling = NullValueHandling.Ignore)] public string Url { get; set; }

    // [JsonProperty("00_image")] public string image { get; set; }   // Commented out because this, for some reason, only works on RPCS3
    // [JsonProperty("00_image_full")] public string image { get; set; } // Will make the game try to get an image from "<ip>/"
}