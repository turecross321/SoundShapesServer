using Newtonsoft.Json;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UserMetadataResponse : IResponse
{
    public UserMetadataResponse(GameUser user)
    {
        Username = user.Username;
        Following = user.Following.Count();
        Followers = user.Followers.Count();
        Levels = user.Levels.Count();
        LikedAndQueuedLevels = user.LikedLevels.Count() + user.QueuedLevels.Count();
    }

    public UserMetadataResponse()
    {
        Username = "";
    }

    [JsonProperty("displayName")] public string Username { get; set; }
    [JsonProperty("follows_by_ever_count")] public int Following { get; set; }
    [JsonProperty("follows_of_ever_count")] public int Followers { get; }
    [JsonProperty("levels_by_ever_count")] public int Levels { get; set; }
    [JsonProperty("likes_by_ever_count")] public int LikedAndQueuedLevels { get; set; }
}