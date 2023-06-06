using Bunkum.HttpServer.Authentication;
using Realms;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Sessions;

public class GameSession : RealmObject, IToken
{
    [Required] [PrimaryKey] public string Id { get; init; }
    public GameUser User { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _SessionType { get; set; }
    public SessionType SessionType
    {
        get => (SessionType)_SessionType;
        set => _SessionType = (int)value;
    }
    
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; set; }
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        set => _PlatformType = (int)value;
    }
    public IpAuthorization? Ip { get; init; }
    public DateTimeOffset ExpiresAt { get; init; }
}