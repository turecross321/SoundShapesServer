using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserResponse
{
    public ApiUserResponse(GameUser userToCheck)
    {
        Id = userToCheck.Id;
        Username = userToCheck.Username;
        PermissionsType = userToCheck.PermissionsType;
        FollowerCount = userToCheck.Followers.Count();
        FollowingCount = userToCheck.Following.Count();
        LikedLevelsCount = userToCheck.LikedLevels.Count();
        PublishedLevelsCount = userToCheck.Levels.Count();
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int FollowerCount { get; set; }
    public int FollowingCount { get; set; }
    public int LikedLevelsCount { get; set; }
    public int PublishedLevelsCount { get; set; }
}