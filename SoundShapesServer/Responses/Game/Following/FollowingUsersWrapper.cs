using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowingUsersWrapper
{
    [JsonProperty("items")] public FollowingUserResponse[] Users { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}