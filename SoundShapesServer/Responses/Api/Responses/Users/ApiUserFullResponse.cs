using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserFullResponse : IApiResponse
{
    public ApiUserFullResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = user.PermissionsType;
        CreationDate = user.CreationDate.ToUnixTimeSeconds();
        LastGameLogin = user.LastGameLogin.ToUnixTimeSeconds();
        LastEventDate = user.Events.Last().CreationDate.ToUnixTimeSeconds();
        Followers = user.Followers.Count();
        Following = user.Following.Count();
        LikedLevels = user.LikedLevels.Count();
        QueuedLevels = user.QueuedLevels.Count();
        PublishedLevels = user.Levels.Count();
        TotalEvents = user.Events.Count();
        PlayedLevels = user.PlayedLevels.Count();
        TotalDeaths = user.Deaths;
        TotalPlayTime = user.TotalPlayTime;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public PermissionsType PermissionsType { get; set; }
    public long CreationDate { get; set; }
    public long LastGameLogin { get; set; }
    public long LastEventDate { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int LikedLevels { get; set; }
    public int QueuedLevels { get; set; }
    public int PublishedLevels { get; set; }
    public int PlayedLevels { get; set; }
    public int TotalEvents { get; set; }
    public int TotalDeaths { get; set; }
    public long TotalPlayTime { get; set; }
}