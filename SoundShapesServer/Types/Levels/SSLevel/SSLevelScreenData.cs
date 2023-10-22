using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SoundShapesServer.Types.Levels.SSLevel;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class SSLevelScreenData
{
    public int ScreenX { get; set; }
    public int ScreenY { get; set; }
    [JsonProperty("colourSchemeID")] public int ColorSchemeId { get; set; }
}