using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums;

public class LinerNotesWrapper
{
    [JsonProperty("version")] public float Version { get; set; }
    [JsonProperty("linerNotes")] public LinerNoteResponse[] LinerNotes { get; set; }
}