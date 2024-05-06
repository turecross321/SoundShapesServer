using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserFullResponse : IApiResponse, IDataConvertableFrom<ApiUserFullResponse, GameUser>
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required PermissionsType PermissionsType { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset? LastGameLogin { get; set; }
    public required DateTimeOffset? LastEventDate { get; set; }
    public required int FollowersCount { get; set; }
    public required int FollowingCount { get; set; }
    public required int LikedLevelsCount { get; set; }
    public required int QueuedLevelsCount { get; set; }
    public required int PublishedLevelsCount { get; set; }
    public required int PlayedLevelsCount { get; set; }
    public required int EventsCount { get; set; }
    public required int TotalDeaths { get; set; }
    public required long TotalPlayTime { get; set; }

    public static ApiUserFullResponse FromOld(GameUser old)
    {
        return new ApiUserFullResponse
        {
            Id = old.Id,
            Username = old.Username,
            PermissionsType = old.PermissionsType,
            CreationDate = old.CreationDate,
            LastGameLogin = old.LastGameLogin,
            LastEventDate = old.Events.LastOrDefault()?.CreationDate,
            FollowersCount = old.FollowersRelations.Count(),
            FollowingCount = old.FollowingRelations.Count(),
            LikedLevelsCount = old.LikedLevelRelations.Count(),
            QueuedLevelsCount = old.QueuedLevelRelations.Count(),
            PublishedLevelsCount = old.Levels.Count(),
            EventsCount = old.Events.Count(),
            PlayedLevelsCount = old.PlayedLevelRelations.Count(),
            TotalDeaths = old.Deaths,
            TotalPlayTime = old.TotalPlayTime
        };
    }

    public static IEnumerable<ApiUserFullResponse> FromOldList(IEnumerable<GameUser> oldList)
    {
        return oldList.Select(FromOld);
    }
}