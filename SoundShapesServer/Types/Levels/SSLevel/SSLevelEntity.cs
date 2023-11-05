using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SoundShapesServer.Types.Levels.SSLevel;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class SSLevelEntity
{
    [JsonProperty("entType")] public string EntityType { get; set; } = null!;
    public int Uid { get; set; }
    public int MinScreenX { get; set; }
    public int MinScreenY { get; set; }
    public int MaxScreenX { get; set; }
    public int MaxScreenY { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }

    /// <summary>
    ///     Rotation value for "entities" entities
    /// </summary>
    public float EditRotateValue { get; set; }

    public string? NoteText { get; set; }
    public int? Z { get; set; }
    public float? EditStretchValue { get; set; }
    [JsonProperty("colourName")] public string? ColorName { get; set; }


    // Specific from entitiesB
    /// <summary>
    ///     Rotation value for "entitiesB" entities
    /// </summary>
    [JsonProperty("rx")]
    public float? RotationX { get; set; }

    [JsonProperty("w")] public float? Width { get; set; }
    [JsonProperty("h")] public float? Height { get; set; }
    public bool? IsNote { get; set; }
    [JsonProperty("ver")] public int? Version { get; set; }
}