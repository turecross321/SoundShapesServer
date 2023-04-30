using Realms;

namespace SoundShapesServer.Types.Albums;

public class LinerNote : EmbeddedObject
{
    public LinerNote(string fontType, string text)
    {
        FontType = fontType;
        Text = text;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public LinerNote() { }
    #pragma warning restore CS8618

    public string FontType { get; set; }
    public string Text { get; set; }
}