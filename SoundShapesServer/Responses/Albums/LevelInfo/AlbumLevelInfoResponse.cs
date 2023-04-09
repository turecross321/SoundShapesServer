using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Albums.LevelInfo;

public class AlbumLevelInfoResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ResponseType.link.ToString();
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public AlbumLevelInfoTarget Target { get; set; }
}