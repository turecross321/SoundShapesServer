using Newtonsoft.Json;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Responses.Game;

public class NewsResponse
{
    public NewsResponse(NewsEntry? entry, bool includeImageUrl)
    {
        if (entry == null) return;
        
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = string.IsNullOrEmpty(Url) ? "0.0.0.0" : entry.Url; // An empty url crashes the Vita version
        if (includeImageUrl) ImageUrl = $"otg/~news:{entry.Id}/~content:thumbnail/data.get";
    }

    public NewsResponse()
    {
        Title = "News";
        Summary = "There are no news yet.";
        FullText = "There are no news yet.";
        Url = "0.0.0.0";
    }

    [JsonProperty("00_title", NullValueHandling = NullValueHandling.Ignore)] public string? Title { get; set; }
    [JsonProperty("00_text", NullValueHandling = NullValueHandling.Ignore)] public string? Summary { get; set; }
    [JsonProperty("00_fullText", NullValueHandling = NullValueHandling.Ignore)] public string? FullText { get; set; }
    [JsonProperty("00_url", NullValueHandling = NullValueHandling.Ignore)] public string? Url { get; set; }
    [JsonProperty("00_image", NullValueHandling = NullValueHandling.Ignore)] public string? ImageUrl { get; set; }
}