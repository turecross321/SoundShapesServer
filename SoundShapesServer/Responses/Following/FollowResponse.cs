using Newtonsoft.Json;
using SoundShapesServer.Responses.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Following;

public class FollowResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; } = ResponseType.follow.ToString();
    [JsonProperty("target")] public UserResponse User { get; set; }
}