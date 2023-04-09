using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfoTarget
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("completed")] public bool Completed { get; set; }
}