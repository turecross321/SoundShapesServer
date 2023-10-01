using Bunkum.Core.Authentication;
using Realms;
using SoundShapesServer.Types.Users;
#pragma warning disable CS8618

namespace SoundShapesServer.Types.Sessions;

public class GameSession : RealmObject, IToken<GameUser>
{
    [Required] [PrimaryKey] public string Id { get; init; }
    public GameUser User { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with SessionType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _SessionType { get; set; }
    public SessionType SessionType
    {
        get => (SessionType)_SessionType;
        set => _SessionType = (int)value;
    }
    
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; set; }
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        set => _PlatformType = (int)value;
    }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ExpiryDate { get; init; }
    public bool? GenuineNpTicket { get; init; }
}