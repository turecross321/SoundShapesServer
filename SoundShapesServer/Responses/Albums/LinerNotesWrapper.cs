using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Albums;

public class LinerNotesWrapper
{
    [JsonProperty("version")] public float Version { get; set; }
    [JsonProperty("linerNotes")] public LinerNoteResponse[] LinerNotes { get; set; }
}