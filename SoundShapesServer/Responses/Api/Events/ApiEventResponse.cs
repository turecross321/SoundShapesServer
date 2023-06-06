using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Responses.Api.Events;

public class ApiEventResponse
{
    public ApiEventResponse(GameDatabaseContext database, GameEvent eventObject)
    {
        Id = eventObject.Id;
        EventType = eventObject.EventType;
        Actor = new ApiUserBriefResponse(eventObject.Actor);
        
        if (eventObject.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(eventObject.ContentUser);
        if (eventObject.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(eventObject.ContentLevel);
        if (eventObject.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(eventObject.ContentLeaderboardEntry, database.GetLeaderboardEntryPosition(eventObject.ContentLeaderboardEntry));
        
        Date = eventObject.Date;
    }

    public string Id { get; set; }
    public int EventType { get; set; }
    public ApiUserBriefResponse Actor { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public DateTimeOffset Date { get; set; }
}