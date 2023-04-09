using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Artist { get; set; }
    public IList<LinerNote> LinerNotes { get; }
    public DateTimeOffset CreationDate { get; set; }
    public IList<GameLevel> Levels { get; }
}