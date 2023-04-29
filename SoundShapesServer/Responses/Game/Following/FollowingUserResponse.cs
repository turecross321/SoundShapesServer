using Newtonsoft.Json;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowingUserResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = GameContentType.follow.ToString();
    [JsonProperty("target")] public UserResponse User { get; set; }
}