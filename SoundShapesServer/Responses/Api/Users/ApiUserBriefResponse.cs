using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserBriefResponse : IApiResponse
{
    public ApiUserBriefResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = (int)user.PermissionsType;
        Followers = user.Followers.Count();
        Following = user.Following.Count();
        PublishedLevels = user.Levels.Count();
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public int PermissionsType { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int PublishedLevels { get; set; }
}