using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Albums.LevelInfo;

public class AlbumLevelInfosWrapper
{
    [JsonProperty("items")] public AlbumLevelInfoResponse[] Items { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}