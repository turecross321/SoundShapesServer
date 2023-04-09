using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelsWrapper
{
    [JsonProperty("items")] public AlbumLevelResponse[] Levels { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}