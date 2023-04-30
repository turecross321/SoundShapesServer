using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.RecentActivity;

public class PlayerNewsResponse
{
    // todo: This obviously shouldn't be a string array, this is just a placeholder.
    public PlayerNewsResponse()
    {
        Items = Array.Empty<string>();
    }

    [JsonProperty("items")] public string[] Items { get; set; }
}