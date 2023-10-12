using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses;

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
        PlatformType = eventObject.PlatformType;
    }

    public string Id { get; set; }
    public EventType EventType { get; set; }
    public ApiUserBriefResponse Actor { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiUserBriefResponse? ContentUser { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLevelBriefResponse? ContentLevel { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public ApiLeaderboardEntryResponse? ContentLeaderboardEntry { get; set; }
    public long CreationDate { get; set; }
    public PlatformType PlatformType { get; set; }
}