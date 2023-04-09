using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumsWrapper
{
    [JsonProperty("items")] public AlbumResponse[] Albums { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}