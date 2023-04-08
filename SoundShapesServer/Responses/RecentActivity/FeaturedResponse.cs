using Newtonsoft.Json;

namespace SoundShapesServer.Responses.RecentActivity;

public class FeaturedResponse
{
    [JsonProperty("00_queryType")] public string queryType { get; set; }
    [JsonProperty("00_buttonLabel")] public string buttonLabel { get; set; }
    [JsonProperty("00_query")] public string query { get; set; }
    [JsonProperty("00_panelDescription")] public string panelDescription { get; set; }
    [JsonProperty("00_imageUrl")] public string imageUrl { get; set; }
    [JsonProperty("00_panelTitle")] public string panelTitle { get; set; }
    
    [JsonProperty("01_queryType")] public string queryType2 { get; set; }
    [JsonProperty("01_buttonLabel")] public string buttonLabel2 { get; set; }
    [JsonProperty("01_query")] public string query2 { get; set; }
    [JsonProperty("01_panelDescription")] public string panelDescription2 { get; set; }
    [JsonProperty("01_imageUrl")] public string imageUrl2 { get; set; }
    [JsonProperty("01_panelTitle")] public string panelTitle2 { get; set; }
}