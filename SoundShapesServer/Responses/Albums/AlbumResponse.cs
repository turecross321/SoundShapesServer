using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Albums;

public class AlbumResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; set; }
    [JsonProperty("timestamp")] public string CreationDate { get; set; }
    [JsonProperty("target")] public AlbumTarget Target { get; set; }
}