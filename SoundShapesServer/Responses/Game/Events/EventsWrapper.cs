using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Responses.Game.Events;

public class EventsWrapper
{
    public EventsWrapper(GameEvent[] events, int totalEvents, int from, int count)
    {
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalEvents, from, count);
        
        Events = events.Select(e=>new EventResponse(e)).ToArray();
    }

    [JsonProperty("items")] public EventResponse[] Events { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}