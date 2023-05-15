using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.PlayerActivity;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class ActivitiesWrapper
{
    public ActivitiesWrapper(GameEvent[] events, int totalEvents, int from, int count)
    {
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalEvents, from, count);
        
        Events = events.Select(e=>new ActivityResponse(e)).ToArray();
    }

    [JsonProperty("items")] public ActivityResponse[] Events { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}