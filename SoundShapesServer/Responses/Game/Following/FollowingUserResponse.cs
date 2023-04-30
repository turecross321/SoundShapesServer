using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowingUserResponse
{
    public FollowingUserResponse(GameUser follower, GameUser followed)
    {
        Id = IdFormatter.FormatFollowId(follower.Id, followed.Id);
        User = new UserResponse(followed);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = GameContentType.follow.ToString();
    [JsonProperty("target")] public UserResponse User { get; set; }
}