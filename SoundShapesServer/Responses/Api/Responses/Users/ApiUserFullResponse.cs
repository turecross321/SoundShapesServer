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
        CreationDate = user.CreationDate;
        LastGameLogin = user.LastGameLogin;
        LastEventDate = user.Events.Last().CreationDate;
        FollowersCount = user.FollowersRelations.Count();
        FollowingCount = user.FollowingRelations.Count();
        LikedLevelsCount = user.LikedLevelRelations.Count();
        QueuedLevelsCount = user.QueuedLevelRelations.Count();
        PublishedLevelsCount = user.Levels.Count();
        EventsCount = user.Events.Count();
        PlayedLevelsCount = user.PlayedLevelRelations.Count();
        TotalDeaths = user.Deaths;
        TotalPlayTime = user.TotalPlayTime;
    }

    public string Id { get; set; }
    public string Username { get; set; }
    public PermissionsType PermissionsType { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset LastGameLogin { get; set; }
    public DateTimeOffset LastEventDate { get; set; }
    public int FollowersCount { get; set; }
    public int FollowingCount { get; set; }
    public int LikedLevelsCount { get; set; }
    public int QueuedLevelsCount { get; set; }
    public int PublishedLevelsCount { get; set; }
    public int PlayedLevelsCount { get; set; }
    public int EventsCount { get; set; }
    public int TotalDeaths { get; set; }
    public long TotalPlayTime { get; set; }
}