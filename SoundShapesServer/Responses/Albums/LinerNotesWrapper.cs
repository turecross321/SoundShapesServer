using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Albums;

public class LinerNotesWrapper
{
    public float version { get; set; }
    public LinerNoteResponse[] linerNotes { get; set; }
}