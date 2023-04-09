using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ResponseType.link.ToString();
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public AlbumLevelTarget Target { get; set; }
}