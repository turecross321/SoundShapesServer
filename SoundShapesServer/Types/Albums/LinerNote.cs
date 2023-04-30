using MongoDB.Bson;
using Realms;

namespace SoundShapesServer.Types.Albums;

public class LinerNote : EmbeddedObject
{
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public LinerNote() { }
    #pragma warning restore CS8618
    public string FontType { get; set; }
    public string Text { get; set; }
}