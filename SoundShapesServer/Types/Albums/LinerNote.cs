using Realms;

namespace SoundShapesServer.Types.Albums;

public class LinerNote : EmbeddedObject
{
    public LinerNote(string fontType, string text)
    {
        FontType = fontType;
        Text = text;
    }

    public string FontType { get; set; }
    public string Text { get; set; }
}