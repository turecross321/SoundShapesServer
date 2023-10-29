using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    public required string Name { get; set; }
    public required GameUser Author { get; set; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ModificationDate { get; set; }
    public required string LinerNotes { get; set; }

    public IList<GameLevel> Levels { get; } = null!;
    
    public string? ThumbnailFilePath { get; set; }
    public string? SidePanelFilePath { get; set; }
}