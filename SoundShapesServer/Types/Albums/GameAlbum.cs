using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    [PrimaryKey] [Required] public string Id { get; init; } // TODO: Sheck...
    public string Name { get; set; }
    public GameUser Author { get; set; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ModificationDate { get; set; }
    public string LinerNotes { get; set; } = null!;

    public IList<GameLevel> Levels { get; } = null!;
    
    public string? ThumbnailFilePath { get; set; }
    public string? SidePanelFilePath { get; set; }
}