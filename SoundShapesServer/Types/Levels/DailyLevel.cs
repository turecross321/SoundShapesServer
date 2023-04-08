using Realms;

namespace SoundShapesServer.Types.Levels;

public class DailyLevel : RealmObject
{
    public GameLevel level { get; set; }
    public DateTimeOffset date { get; set; }
}