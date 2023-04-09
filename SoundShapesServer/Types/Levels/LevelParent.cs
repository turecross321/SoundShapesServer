using Realms;

namespace SoundShapesServer.Types.Levels;

public class LevelParent : EmbeddedObject
{
    public string Id { get; set; }
    public string Type { get; set; }
}