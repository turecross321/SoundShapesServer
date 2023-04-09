using Realms;

namespace SoundShapesServer.Types.Albums;

public class LinerNote : EmbeddedObject
{
    public string FontType { get; set; }
    public string Text { get; set; }
}