using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Albums;

public class AlbumMetadata
{
    [JsonProperty("albumArtist")] public string Artist { get; set; }
    [JsonProperty("linerNotes")] public string LinerNotes { get; set; }
    [JsonProperty("sidePanelURL")] public string SidePanelUrl { get; set; }
    [JsonProperty("date")] public string CreationDate { get; set; }
    [JsonProperty("displayName")] public string Name { get; set; }
    [JsonProperty("thumbnailURL")] public string ThumbnailUrl { get; set; }
}