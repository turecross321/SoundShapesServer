using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types.Albums;

public class GameAlbum : RealmObject
{
    public string id { get; set; }
    public string name { get; set; }
    public string artist { get; set; }
    public string thumbnailURL { get; set; }
    public string sidePanelURL { get; set; }
    public IList<LinerNote> linerNotes { get; }
    public DateTimeOffset date { get; set; }
    public IList<GameLevel> levels { get; }
}