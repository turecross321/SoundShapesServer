using Bunkum.Core.Authentication;
using Realms;
using SoundShapesServer.Types.Users;

#pragma warning disable CS8618

namespace SoundShapesServer.Types.Authentication;

public class GameToken : RealmObject, IToken<GameUser>
{
    [Required] [PrimaryKey] public string Id { get; init; }
    public GameUser User { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _TokenType { get; set; }
    public TokenType TokenType
    {
        get => (TokenType)_TokenType;
        set => _TokenType = (int)value;
    }
    
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; set; }
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        init => _PlatformType = (int)value;
    }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ExpiryDate { get; set; }
    public bool? GenuineNpTicket { get; init; }
    public GameToken? RefreshToken { get; init; }
    [Backlink(nameof(RefreshToken))] public IQueryable<GameToken> RefreshableTokens { get; }
}