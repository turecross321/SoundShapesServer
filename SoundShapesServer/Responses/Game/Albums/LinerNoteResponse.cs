using Newtonsoft.Json;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class LinerNoteResponse : IResponse
{
    public LinerNoteResponse(FontType fontType, string text)
    {
        Font = fontType switch
        {
            FontType.Title => "title",
            FontType.Header => "heading",
            FontType.Normal => "normal",
            _ => "normal"
        };
        
        Text = text;
    }

    [JsonProperty("fontType")] public string Font { get; set; }
    [JsonProperty("text")] public string Text { get; set; }
}