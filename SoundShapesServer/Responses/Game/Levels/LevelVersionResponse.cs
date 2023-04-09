using Newtonsoft.Json;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelVersionResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type { get; } = ResponseType.version.ToString();
}