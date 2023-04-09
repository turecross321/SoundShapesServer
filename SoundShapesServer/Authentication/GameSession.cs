using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameSession : RealmObject, IToken
{
    public DateTimeOffset Expires { get; set; }
    public string Id { get; set; }
    public GameUser User { get; set; }
    public string Platform { get; set; }
}