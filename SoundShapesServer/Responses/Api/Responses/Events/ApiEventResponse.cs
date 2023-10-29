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

namespace SoundShapesServer.Responses.Api.Responses;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiEventResponse : IApiResponse
{
    public ApiEventResponse(GameDatabaseContext database, GameEvent gameEvent)
    {
        Id = gameEvent.Id.ToString()!;
        EventType = gameEvent.EventType;
        Actor = new ApiUserBriefResponse(gameEvent.Actor);
        DataType = gameEvent.DataType;
        CreationDate = gameEvent.CreationDate;
        PlatformType = gameEvent.PlatformType;

        switch (gameEvent.DataType)
        {
            case EventDataType.Level:
                DataLevel = new ApiLevelBriefResponse((GameLevel)gameEvent.Data(database));
                break;
            case EventDataType.LeaderboardEntry:
                LeaderboardEntry entry = (LeaderboardEntry)gameEvent.Data(database);
                LeaderboardFilters filters = new LeaderboardFilters { OnLevel = entry.Level, Completed = entry.Completed, Obsolete = entry.Obsolete() };
                LeaderboardOrderType order = LeaderboardOrderType.Notes;
                DataLeaderboardEntry = new ApiLeaderboardEntryResponse(entry, order, filters);
                break;
            case EventDataType.User:
                DataUser = new ApiUserBriefResponse((GameUser)gameEvent.Data(database));
                break;
            case EventDataType.Album:
                DataAlbum = new ApiAlbumResponse((GameAlbum)gameEvent.Data(database));
                break;
            case EventDataType.NewsEntry:
                DataNewsEntry = new ApiNewsEntryResponse((NewsEntry)gameEvent.Data(database));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public string Id { get; set; }
    public EventType EventType { get; set; }
    public ApiUserBriefResponse Actor { get; set; }

    public DateTimeOffset CreationDate { get; set; }
    public PlatformType PlatformType { get; set; }
    public EventDataType DataType { get; set; }
    
    public ApiLevelBriefResponse? DataLevel { get; set; }
    public ApiLeaderboardEntryResponse? DataLeaderboardEntry { get; set; }
    public ApiUserBriefResponse? DataUser { get; set; }
    public ApiAlbumResponse? DataAlbum { get; set; }
    public ApiNewsEntryResponse? DataNewsEntry { get; set; }
}