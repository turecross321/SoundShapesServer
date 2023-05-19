using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserFullResponse
{
    public ApiUserFullResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = user.PermissionsType;
        CreationDate = user.CreationDate;
        FollowersCount = user.Followers.Count();
        FollowingCount = user.Following.Count();
        LikedLevelsCount = user.LikedLevels.Count();
        PublishedLevelsCount = user.Levels.Count();
        ActivitiesCount = user.Events.Count();
        PlayedLevelsCount = user.PlayedLevels.Count();
        TotalDeaths = user.Deaths;
        TotalPlayTime = user.TotalPlayTime;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int LikedLevelsCount { get; set; }
    public int PublishedLevelsCount { get; set; }
    public int ActivitiesCount { get; set; }
    public int PlayedLevelsCount { get; set; }
    public int TotalDeaths { get; set; }
    public long TotalPlayTime { get; set; }
}