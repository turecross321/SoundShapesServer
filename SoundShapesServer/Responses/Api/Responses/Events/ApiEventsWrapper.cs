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

public class ApiEventsWrapper : IApiResponse
{
    public ApiEventsWrapper(GameDatabaseContext database, PaginatedList<GameEvent> events)
    {
        Events = ApiEventResponse.FromOldList(events.Items);
        DataLevels = new List<ApiLevelBriefResponse>();
        DataLeaderboardEntries = new List<ApiLeaderboardEntryResponse>();
        DataUsers = new List<ApiUserBriefResponse>();
        DataAlbums = new List<ApiAlbumResponse>();
        DataNewsEntries = new List<ApiNewsEntryResponse>();

        foreach (GameEvent gameEvent in events.Items)
            switch (gameEvent.DataType)
            {
                case EventDataType.Level:
                    DataLevels.Add(ApiLevelBriefResponse.FromOld((GameLevel)gameEvent.Data(database)));
                    break;
                case EventDataType.LeaderboardEntry:
                    LeaderboardEntry entry = (LeaderboardEntry)gameEvent.Data(database);
                    LeaderboardFilters filters = new()
                    {
                        Completed = entry.Completed,
                        Obsolete = entry.Obsolete()
                    };
                    DataLeaderboardEntries.Add(
                        ApiLeaderboardEntryResponse.FromOld(entry, LeaderboardOrderType.Score, filters));
                    break;
                case EventDataType.User:
                    DataUsers.Add(ApiUserBriefResponse.FromOld((GameUser)gameEvent.Data(database)));
                    break;
                case EventDataType.Album:
                    DataAlbums.Add(ApiAlbumResponse.FromOld((GameAlbum)gameEvent.Data(database)));
                    break;
                case EventDataType.NewsEntry:
                    DataNewsEntries.Add(ApiNewsEntryResponse.FromOld((NewsEntry)gameEvent.Data(database)));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    public IEnumerable<ApiEventResponse> Events { get; set; } = null!;
    public List<ApiLevelBriefResponse> DataLevels { get; set; } = null!;
    public List<ApiLeaderboardEntryResponse> DataLeaderboardEntries { get; set; } = null!;
    public List<ApiUserBriefResponse> DataUsers { get; set; } = null!;
    public List<ApiAlbumResponse> DataAlbums { get; set; } = null!;
    public List<ApiNewsEntryResponse> DataNewsEntries { get; set; } = null!;
}