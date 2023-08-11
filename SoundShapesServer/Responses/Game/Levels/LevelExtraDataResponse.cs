using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelExtraDataResponse : IResponse
{
    public LevelExtraDataResponse(int language)
    {
        Language = language;
    }
    [JsonProperty("sce_np_language")] public int Language { get; set; }
}