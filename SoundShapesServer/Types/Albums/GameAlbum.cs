using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Artist { get; set; } = "";
    // this is because album creation isnt implemented yet!
    public IList<LinerNote> LinerNotes { get; } = new List<LinerNote>();
    public DateTimeOffset CreationDate { get; set; }
    public IList<GameLevel> Levels { get; } = new List<GameLevel>();
}