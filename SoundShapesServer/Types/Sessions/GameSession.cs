using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Sessions;

public class GameSession : RealmObject, IToken
{
    [Required] [PrimaryKey] public string Id { get; init; } = "";
    
    #pragma warning disable CS8618
    public GameUser User { get; init; } // there's gotta be a cleaner way of doing this
    #pragma warning restore CS8618
    public int SessionType { get; init; }
    public int? PlatformType { get; init; }
    public IpAuthorization? Ip { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}