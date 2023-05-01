using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Artist { get; set; } = "";
    public DateTimeOffset CreationDate { get; set; }
    public string LinerNotes { get; set; } = "";
    
    // ReSharper disable UnassignedGetOnlyAutoProperty
    #pragma warning disable CS8618
    public IList<GameLevel> Levels { get; }
    #pragma warning restore CS8618
    // ReSharper restore UnassignedGetOnlyAutoProperty
}