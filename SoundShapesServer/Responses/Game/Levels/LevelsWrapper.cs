using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelsWrapper
{
    [JsonProperty("items")] public LevelResponse[] Levels { get; set; }
    [JsonProperty("count")] public int LevelCount { get; set; }
    
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? NextToken { get; set; }
}