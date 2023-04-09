using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class Session : RealmObject, IToken
{
    public DateTimeOffset ExpiresAt { get; set; }
    public string Id { get; set; }
    public GameUser User { get; set; }
    public string Platform { get; set; }
}