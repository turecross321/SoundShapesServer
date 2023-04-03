using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Levels;
public class LevelResponsesWrapper
{
    public LevelResponse[] items { get; set; }
    public int count { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public int? nextToken { get; set; }
}