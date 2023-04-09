using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Levels;

public class ExtraDataResponse
{
    [JsonProperty("sce_np_language")] public int Language { get; set; }
}