using Newtonsoft.Json;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiPlayerActivityResponse
{
    public ApiPlayerActivityResponse(GameEvent eventObject)
    {
        Id = eventObject.Id;
        EventType = eventObject.EventType;
        ActorId = eventObject.Actor.Id;

        UserId = eventObject.ContentUser?.Id;
        LevelId = eventObject.ContentLevel?.Id;
        LeaderboardEntryId = eventObject.ContentLeaderboardEntry?.Id;
        
        Date = eventObject.Date;
    }

    public string Id { get; set; }
    public int EventType { get; set; }
    public string ActorId { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? UserId { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? LevelId { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public string? LeaderboardEntryId { get; set; }
    public DateTimeOffset Date { get; set; }
}