using Realms;
using SoundShapesServer.Enums;

namespace SoundShapesServer.Types.Albums;

public class LinerNote : EmbeddedObject
{
    public string fontType { get; set; }
    public string text { get; set; }
}