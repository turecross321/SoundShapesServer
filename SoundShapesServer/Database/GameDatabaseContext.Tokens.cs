using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.TokenHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public const int DefaultTokenExpirySeconds = Globals.OneDayInSeconds;

    public GameToken CreateToken(GameUser user, TokenType tokenType, 
        double expirationSeconds = DefaultTokenExpirySeconds, PlatformType platformType = PlatformType.Unknown, 
        bool? genuineTicket = null, GameToken? refreshToken = null)
    {
        string id = tokenType switch
        {
            TokenType.SetPassword => GenerateSimpleTokenId(this, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", 8, TokenType.SetPassword),
            TokenType.SetEmail => GenerateSimpleTokenId(this, "123456789!#¤%&/()=?", 8, TokenType.SetEmail),
            TokenType.AccountRemoval => GenerateSimpleTokenId(this,
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789", 8, TokenType.AccountRemoval),
            TokenType.AccountRegistration => GenerateSimpleTokenId(this, "123456789", 8, TokenType.SetEmail),
            _ => GenerateGuid()
        };
        
        GameToken token = new()
        {
            Id = id,
            User = user,
            TokenType = tokenType,
            PlatformType = platformType,
            ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(expirationSeconds),
            CreationDate = DateTimeOffset.UtcNow,
            GenuineNpTicket = genuineTicket,
            RefreshToken = refreshToken
        };

        _realm.Write(() =>
        {
            _realm.Add(token);
            if (tokenType == TokenType.GameAccess) 
                user.LastGameLogin = DateTimeOffset.UtcNow;
        });

        _realm.Refresh();
        
        return token;
    }

    public void RefreshToken(GameToken token, long expirySecondsFromNow)
    {
        _realm.Write(() =>
        {
            token.ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(expirySecondsFromNow);
        });
    }

    public void RemoveToken(GameToken token)
    {
        _realm.Write(() =>
        {
            _realm.Remove(token);
        });
    }

    public void RemoveToken(IQueryable<GameToken> token)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(token);
        });
    }

    private void RemoveTokensByUser(GameUser user)
    {
        _realm.Write(() =>
        {
            _realm.RemoveRange(user.Tokens);
        });
    }
    
    // null type = all
    public GameToken? GetTokenWithId(string tokenId, TokenType? type)
    {
        IQueryable<GameToken> tokens = _realm.All<GameToken>();
        if (type != null)
            tokens = tokens.Where(t => t._TokenType == (int)type);
        
        return tokens.FirstOrDefault(t => t.Id == tokenId);
    }
}