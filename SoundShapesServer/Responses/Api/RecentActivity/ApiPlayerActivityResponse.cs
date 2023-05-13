using Newtonsoft.Json;
using Realms.Sync;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivityResponse
{
    public ApiPlayerActivityResponse(GameEvent eventObject)
    {
        Id = eventObject.Id;
        EventType = eventObject.EventType;
        ActorId = eventObject.Actor.Id;
        ActorUsername = eventObject.Actor.Username;

        ContentUserId = eventObject.ContentUser?.Id;
        ContentUsername = eventObject.ContentUser?.Username;
        ContentLevelId = eventObject.ContentLevel?.Id;
        ContentLevelName = eventObject.ContentLevel?.Name;
        ContentLeaderboardEntryId = eventObject.ContentLeaderboardEntry?.Id;
        
        Date = eventObject.Date;
    }

    public string Id { get; set; }
    public int EventType { get; set; }
    public string ActorId { get; set; }
    public string ActorUsername { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? ContentUserId { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? ContentUsername { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? ContentLevelId { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? ContentLevelName { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? ContentLeaderboardEntryId { get; set; }
    public DateTimeOffset Date { get; set; }
}