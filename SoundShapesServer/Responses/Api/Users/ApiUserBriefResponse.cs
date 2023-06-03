using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserBriefResponse
{
    public ApiUserBriefResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = user.PermissionsType;
        PublishedLevels = user.Levels.Count();
        Followers = user.Followers.Count();
    }

#pragma warning disable CS8618
    public ApiUserBriefResponse() {}
#pragma warning restore CS8618


    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int PublishedLevels { get; set; }
    public int Followers { get; set; }
}