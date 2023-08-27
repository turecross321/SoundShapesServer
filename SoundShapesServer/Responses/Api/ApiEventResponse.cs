using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api;

public class ApiEventResponse : IApiResponse
{
    public ApiEventResponse(GameDatabaseContext database, GameEvent eventObject, GameUser? accessor)
    {
        Id = eventObject.Id;
        EventType = eventObject.EventType;
        Actor = new ApiUserBriefResponse(eventObject.Actor);
        
        if (eventObject.ContentUser != null)
            ContentUser = new ApiUserBriefResponse(eventObject.ContentUser);
        else if (eventObject.ContentLeaderboardEntry != null)
            ContentLeaderboardEntry = new ApiLeaderboardEntryResponse(eventObject.ContentLeaderboardEntry, database.GetLeaderboardEntryPosition(eventObject.ContentLeaderboardEntry, accessor));
        else if (eventObject.ContentLevel != null)
            ContentLevel = new ApiLevelBriefResponse(eventObject.ContentLevel);
        
        CreationDate = eventObject.CreationDate.ToUnixTimeSeconds();
    }

    public string Id { get; set; }
    public EventType EventType { get; set; }
    public ApiUserBriefResponse Actor { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public long CreationDate { get; set; }
}