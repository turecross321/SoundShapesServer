using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Events;

public class EventLevelVersionResponse : IResponse
{
    // This is only used with recent activity levels
    public EventLevelVersionResponse(string id)
    {
        Id = id;
    }

    [JsonProperty("id")] public string Id { get; set; }
}