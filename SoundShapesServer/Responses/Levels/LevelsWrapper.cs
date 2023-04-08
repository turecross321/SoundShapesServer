using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Levels;
public class LevelsWrapper
{
    public LevelResponse[] items { get; set; }
    public int count { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? previousToken { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? nextToken { get; set; }
}