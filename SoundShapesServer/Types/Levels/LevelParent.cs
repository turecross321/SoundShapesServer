using Realms;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Types.Levels;

public class LevelParent : EmbeddedObject
{
    public LevelParent(GameLevel level)
    {
        Id = IdFormatter.FormatLevelId(level.Id);
        Type = GameContentType.level.ToString();
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public LevelParent() { }
    #pragma warning restore CS8618

    public string Id { get; set; }
    public string Type { get; set; }
}