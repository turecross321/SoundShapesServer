using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserBriefResponse : IApiResponse
{
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiUserBriefResponse() {}
    public ApiUserBriefResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = (int)user.PermissionsType;
        FollowersCount = user.FollowersRelations.Count();
        FollowingCount = user.FollowingRelations.Count();
        PublishedLevelsCount = user.Levels.Count();
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int PublishedLevelsCount { get; set; }
}