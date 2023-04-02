using Realms;

namespace SoundShapesServer.Types.Levels;

public class LevelResources : EmbeddedObject
{
    public string imageFileUrl { get; set; }
    public string levelFileUrl { get; set; }
    public string soundFileUrl { get; set; }
}