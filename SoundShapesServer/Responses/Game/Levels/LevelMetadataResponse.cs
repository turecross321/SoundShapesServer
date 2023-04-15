using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelMetadataResponse
{
    [JsonProperty("unique_plays_ever_count")] public string UniquePlaysCount { get; set; }  
    [JsonProperty("difficulty")] public string Difficulty { get; set; }
    [JsonProperty("timestamp")] public string Timestamp { get; set; }  
    [JsonProperty("plays_of_ever_count")] public string TotalPlaysCount { get; set; }
    [JsonProperty("displayName")] public string Name { get; set; }
    [JsonProperty("sce_np_language")] public string Language { get; set; }  
    [JsonProperty("likes_of_ever_count")] public string LikesCount { get; set; }
}