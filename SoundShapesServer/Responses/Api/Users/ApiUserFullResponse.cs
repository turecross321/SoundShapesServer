using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUserFullResponse
{
    public ApiUserFullResponse(GameUser user)
    {
        Id = user.Id;
        Username = user.Username;
        PermissionsType = user.PermissionsType;
        CreationDate = user.CreationDate;
        LastGameLogin = user.LastGameLogin;
        LastEventDate = user.Events.Last().Date;
        Followers = user.Followers.Count();
        Following = user.Following.Count();
        LikedLevels = user.LikedLevels.Count();
        QueuedLevels = user.QueuedLevels.Count();
        PublishedLevels = user.Levels.Count();
        Events = user.Events.Count();
        PlayedLevels = user.PlayedLevels.Count();
        TotalDeaths = user.Deaths;
        TotalPlayTime = user.TotalPlayTime;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public PermissionsType PermissionsType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset LastGameLogin { get; set; }
    public DateTimeOffset LastEventDate { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public int LikedLevels { get; set; }
    public int QueuedLevels { get; set; }
    public int PublishedLevels { get; set; }
    public int Events { get; set; }
    public int PlayedLevels { get; set; }
    public int TotalDeaths { get; set; }
    public long TotalPlayTime { get; set; }
}