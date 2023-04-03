using Newtonsoft.Json;

namespace SoundShapesServer.Responses;

public class FollowingUserResponsesWrapper
{
    public FollowingUserResponse[] items { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public int? nextToken { get; set; }
    
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)]
    public int? previousToken { get; set; }
}