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
        Followers = user.FollowersRelations.Count();
        Following = user.FollowingRelations.Count();
        PublishedLevels = user.Levels.Count();
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int PublishedLevels { get; set; }
}