using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Levels;

public class ExtraDataResponse
{
    [JsonProperty("sce_np_language")] public int Language { get; set; }
}