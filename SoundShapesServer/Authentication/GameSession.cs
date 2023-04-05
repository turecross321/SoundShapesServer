using Realms;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameSession : RealmObject
{
    public DateTimeOffset expires { get; set; }
    public string id { get; set; }
    public GameUser user { get; set; }
    public string platform { get; set; }
}