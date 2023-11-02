using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Albums;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses.Events;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiEventResponse : IApiResponse
{
    public required string Id { get; set; }
    public required EventType EventType { get; set; }
    public required ApiUserBriefResponse Actor { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required PlatformType PlatformType { get; set; }
    public required ApiLevelBriefResponse? DataLevel { get; set; }
    public required ApiLeaderboardEntryResponse? DataLeaderboardEntry { get; set; }
    public required ApiUserBriefResponse? DataUser { get; set; }
    public required ApiAlbumResponse? DataAlbum { get; set; }
    public required ApiNewsEntryResponse? DataNewsEntry { get; set; }

    public static IEnumerable<ApiEventResponse> FromOldList(GameDatabaseContext database,
        IEnumerable<GameEvent> oldList)
    {
        return oldList.Select(e => FromOld(database, e));
    }

    public static ApiEventResponse FromOld(GameDatabaseContext database, GameEvent old)
    {
        ApiLevelBriefResponse? dataLevel = null;
        ApiLeaderboardEntryResponse? dataLeaderboardEntry = null;
        ApiUserBriefResponse? dataUser = null;
        ApiAlbumResponse? dataAlbum = null;
        ApiNewsEntryResponse? dataNewsEntry = null;

        switch (old.DataType)
        {
            case EventDataType.Level:
                dataLevel = ApiLevelBriefResponse.FromOld((GameLevel)old.Data(database));
                break;
            case EventDataType.LeaderboardEntry:
                LeaderboardEntry entry = (LeaderboardEntry)old.Data(database);
                LeaderboardFilters filters = new()
                {
                    Completed = entry.Completed,
                    Obsolete = false
                };
                dataLeaderboardEntry =
                    ApiLeaderboardEntryResponse.FromOld(entry, LeaderboardOrderType.Score, filters);
                dataLevel = ApiLevelBriefResponse.FromOld(entry.Level);
                break;
            case EventDataType.User:
                dataUser = ApiUserBriefResponse.FromOld((GameUser)old.Data(database));
                break;
            case EventDataType.Album:
                dataAlbum = ApiAlbumResponse.FromOld((GameAlbum)old.Data(database));
                break;
            case EventDataType.NewsEntry:
                dataNewsEntry = ApiNewsEntryResponse.FromOld((NewsEntry)old.Data(database));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        return new ApiEventResponse
        {
            Id = old.Id.ToString()!,
            EventType = old.EventType,
            Actor = ApiUserBriefResponse.FromOld(old.Actor),
            CreationDate = old.CreationDate,
            PlatformType = old.PlatformType,
            DataLevel = dataLevel,
            DataLeaderboardEntry = dataLeaderboardEntry,
            DataUser = dataUser,
            DataAlbum = dataAlbum,
            DataNewsEntry = dataNewsEntry
        };
    }
}