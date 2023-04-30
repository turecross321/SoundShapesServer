using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types;

namespace SoundShapesServer.Authentication;

public class GameSession : RealmObject, IToken
{
    public string? Id { get; init; }
    public GameUser? User { get; init; }
    public int SessionType { get; init; }
    public IpAuthorization? Ip { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}