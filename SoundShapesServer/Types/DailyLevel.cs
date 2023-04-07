using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class DailyLevel : RealmObject
{
    public GameLevel level { get; set; }
    public DateTimeOffset date { get; set; }
}