using Newtonsoft.Json;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class ActivitiesWrapper
{
    public ActivitiesWrapper(IQueryable<GameEvent> events, int from, int count)
    {
        GameEvent[] paginatedEvents = PaginationHelper.PaginateEvents(events, from, count);
        ActivityResponse[] activityResponses = paginatedEvents.Select(e => new ActivityResponse(e)).ToArray();

        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(events.Count(), from, count);
        
        Events = activityResponses;
        PreviousToken = previousToken;
        NextToken = nextToken;
    }

    [JsonProperty("items")] public ActivityResponse[] Events { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}