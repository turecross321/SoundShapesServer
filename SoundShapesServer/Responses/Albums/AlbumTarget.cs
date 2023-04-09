using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Albums;

public class AlbumTarget
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ResponseType.album.ToString();
    [JsonProperty("metadata")] public AlbumMetadata Metadata { get; set; }
}