using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Authentication;

public class GameSession : RealmObject, IToken
{
    [Required] [PrimaryKey] public string Id { get; init; } = "";
    public GameUser? User { get; init; }
    public int SessionType { get; init; }
    public int? PlatformType { get; init; }
    public IpAuthorization? Ip { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}