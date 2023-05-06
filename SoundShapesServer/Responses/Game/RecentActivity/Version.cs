using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class Version
{
    // This is only used with recent activity levels
    public Version(string id)
    {
        Id = id;
    }

    [JsonProperty("id")] public string Id { get; set; }
}