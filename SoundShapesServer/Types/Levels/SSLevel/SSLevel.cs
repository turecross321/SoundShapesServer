using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Types.Levels.SSLevel;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class SSLevel
{
    public static SSLevel? FromLevelFile(byte[] level)
    {
        using MemoryStream stream = new(level);
        string? json = ResourceHelper.DecompressZlib(stream);
        return json == null ? null : JsonConvert.DeserializeObject<SSLevel>(json);
    }
    
    public int FormatVersion { get; set; }
    public int Bpm { get; set; }
    public int ScaleIndex { get; set; }
    public int TransposeValue { get; set; }
    public float PlayerX { get; set; }
    public float PlayerY { get; set; }
    public int FirstScreenX { get; set; }
    public int FirstScreenY { get; set; }
    public IEnumerable<string> EntityTypesUsed { get; set; }
    public IEnumerable<SSLevelEntity> Entities { get; set; }
    public IEnumerable<SSLevelEntity> EntitiesB { get; set; }
    public IEnumerable<SSLevelScreenData> ScreenData { get; set; }
    public int Version { get; set; }
}