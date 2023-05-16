using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.PlayerActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivityResponse
{
    public ApiPlayerActivityResponse(GameDatabaseContext database, GameEvent eventObject)
    {
        Id = eventObject.Id;
        EventType = eventObject.EventType;
        Actor = new ApiUserBriefResponse(eventObject.Actor);
        
        if (eventObject.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(eventObject.ContentUser);
        if (eventObject.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(eventObject.ContentLevel);
        if (eventObject.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(eventObject.ContentLeaderboardEntry, database.GetEntryPlacement(eventObject.ContentLeaderboardEntry));
        
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