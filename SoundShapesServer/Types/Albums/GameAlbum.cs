using Realms;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public GameAlbum(string id, ApiCreateAlbumRequest request, DateTimeOffset date, IList<GameLevel> levels)
    {
        Id = id;
        Name = request.Name;
        Author = request.Author;
        CreationDate = date;
        ModificationDate = date;
        LinerNotes = request.LinerNotes;
        Levels = levels;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameAlbum() { }
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; set; }
    public string Name { get; set; }
    public string Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public string LinerNotes { get; set; }
    
    // ReSharper disable UnassignedGetOnlyAutoProperty
    #pragma warning disable CS8618
    public IList<GameLevel> Levels { get; }
    #pragma warning restore CS8618
    // ReSharper restore UnassignedGetOnlyAutoProperty
    
    public string? ThumbnailFilePath { get; set; }
    public string? SidePanelFilePath { get; set; }
}