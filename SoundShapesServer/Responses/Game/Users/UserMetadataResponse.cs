using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Users;

public class UserMetadataResponse
{
    [JsonProperty("displayName")] public string DisplayName { get; set; } = "";
    [JsonProperty("follows_of_ever_count")] public int FanCount { get; set; } = 0;
    [JsonProperty("levels_by_ever_count")] public int LevelCount { get; set; } = 0;
    [JsonProperty("follows_by_ever_count")] public int FollowingCount { get; set; } = 0;
    [JsonProperty("likes_by_ever_count")] public int LikedLevelsCount { get; set; } = 0;
}