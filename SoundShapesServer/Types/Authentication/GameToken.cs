using Bunkum.Core.Authentication;
using Realms;
using SoundShapesServer.Types.Users;

#pragma warning disable CS8618

namespace SoundShapesServer.Types.Authentication;

public class GameToken : RealmObject, IToken<GameUser>
{
    [Required] [PrimaryKey] public required string Id { get; init; }
    public required GameUser User { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _TokenType { get; set; }
    public required TokenType TokenType
    {
        get => (TokenType)_TokenType;
        set => _TokenType = (int)value;
    }
    
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; set; }
    public required PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        init => _PlatformType = (int)value;
    }
    
    // ReSharper disable once InconsistentNaming
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _TokenAuthenticationType { get; set; }

    public required TokenAuthenticationType TokenAuthenticationType
    {
        get => (TokenAuthenticationType)_TokenAuthenticationType;
        set => _TokenAuthenticationType = (int)value;
    }
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset ExpiryDate { get; set; }
    public bool? GenuineNpTicket { get; init; }
    public GameToken? RefreshToken { get; init; }
    [Backlink(nameof(RefreshToken))] public IQueryable<GameToken> RefreshableTokens { get; } = null!;
}