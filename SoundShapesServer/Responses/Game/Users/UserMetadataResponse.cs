using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Users;

public class UserMetadataResponse
{
    public UserMetadataResponse(GameUser user)
    {
        DisplayName = user.Username;
        FollowingCount = user.Following.Count();
        FollowersCount = user.Followers.Count();
        LevelCount = user.Levels.Count();
        LikedLevelsCount = user.LikedLevels.Count();
    }

    public UserMetadataResponse()
    {
        DisplayName = "";
    }

    [JsonProperty("displayName")] public string DisplayName { get; set; }
    [JsonProperty("follows_by_ever_count")] public int FollowingCount { get; set; }
    [JsonProperty("follows_of_ever_count")] public int FollowersCount { get; }
    [JsonProperty("levels_by_ever_count")] public int LevelCount { get; set; }
    [JsonProperty("likes_by_ever_count")] public int LikedLevelsCount { get; set; }
}