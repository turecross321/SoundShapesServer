using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public GameAlbum(string id, string name, string artist, IList<LinerNote> linerNotes, DateTimeOffset creationDate, IList<GameLevel> levels)
    {
        Id = id;
        Name = name;
        Artist = artist;
        LinerNotes = linerNotes;
        CreationDate = creationDate;
        Levels = levels;
    }
    
    // Realm cries if this doesn't exist
    #pragma warning disable CS8618
    public GameAlbum() { }
    #pragma warning restore CS8618
    
    public string Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public IList<LinerNote> LinerNotes { get; }
    public DateTimeOffset CreationDate { get; set; }
    public IList<GameLevel> Levels { get; }
}