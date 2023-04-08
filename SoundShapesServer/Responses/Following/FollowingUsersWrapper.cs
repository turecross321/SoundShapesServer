using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Following;

public class FollowingUsersWrapper
{
    public FollowingUserResponse[] items { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? previousToken { get; set; }
    [JsonProperty(NullValueHandling=NullValueHandling.Ignore)] public int? nextToken { get; set; }
}