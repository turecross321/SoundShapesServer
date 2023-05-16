using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserFullResponse
{
    public ApiUserFullResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = user.PermissionsType;
        FollowersCount = user.Followers.Count();
        FollowingCount = user.Following.Count();
        LikedLevelsCount = user.LikedLevels.Count();
        PublishedLevelsCount = user.Levels.Count();
        Deaths = user.Deaths;
        TotalPlayTime = user.TotalPlayTime;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int LikedLevelsCount { get; set; }
    public int PublishedLevelsCount { get; set; }
    public int Deaths { get; set; }
    public long TotalPlayTime { get; set; }
}