using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public GameLevel Level { get; set; }
    public DateTimeOffset Date { get; set; }
}