using Realms;

namespace SoundShapesServer.Types.Levels;

public class LevelParent : EmbeddedObject
{
    public string id { get; set; }
    public string type { get; set; }
}