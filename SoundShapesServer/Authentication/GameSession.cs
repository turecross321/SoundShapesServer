using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameSession : RealmObject, IToken
{
    public string Id { get; set; }
    public GameUser User { get; set; }
    public int SessionType { get; set; }
    public IpAuthorization Ip { get; set; }
    public DateTimeOffset ExpiresAt { get; set; }
}