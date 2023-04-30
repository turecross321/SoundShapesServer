using Newtonsoft.Json;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class LinerNoteResponse
{
    public LinerNoteResponse(LinerNote linerNote)
    {
        FontType = linerNote.FontType;
        Text = linerNote.Text;
    }

    [JsonProperty("fontType")] public string FontType { get; set; }
    [JsonProperty("text")] public string Text { get; set; }
}