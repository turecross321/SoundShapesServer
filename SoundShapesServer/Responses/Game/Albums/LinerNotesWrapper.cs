using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Game.Albums;

public class LinerNotesWrapper : IResponse
{
    public LinerNotesWrapper(LinerNoteResponse[] linerNotes)
    {
        LinerNotes = linerNotes;
        Version = 1;
    }

    [JsonProperty("version")] public float Version { get; set; }
    [JsonProperty("linerNotes")] public LinerNoteResponse[] LinerNotes { get; set; }
}