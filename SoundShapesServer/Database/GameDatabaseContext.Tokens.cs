using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.TokenHelper;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public const int DefaultTokenExpirySeconds = Globals.OneDayInSeconds;
    private const int SimultaneousTokensLimit = 3;

    public AuthToken CreateToken(GameUser user, TokenType tokenType, 
        double expirationSeconds = DefaultTokenExpirySeconds, PlatformType platformType = PlatformType.Unknown, 
        bool? genuineTicket = null, AuthToken? refreshToken = null)
    {
        string id = tokenType switch
        {
            TokenType.SetPassword => GeneratePasswordTokenId(this),
            TokenType.SetEmail => GenerateEmailTokenId(this),
            TokenType.AccountRemoval => GenerateAccountRemovalTokenId(this),
            _ => GenerateGuid()
        };
        
        AuthToken token = new()
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

        IEnumerable<AuthToken> tokensToDelete = _realm.All<AuthToken>()
            .Where(s=> s.User == user && s._TokenType == (int)tokenType)
            .AsEnumerable()
            .SkipLast(SimultaneousTokensLimit - 1);

        _realm.Write(() =>
        {
            foreach (AuthToken tokenToDelete in tokensToDelete)
            {
                _realm.Remove(tokenToDelete);
            }
            
            _realm.Add(token);
            if (tokenType == TokenType.GameAccess) 
                user.LastGameLogin = DateTimeOffset.UtcNow;
        });

        _realm.Refresh();
        
        return token;
    }

    public void RefreshToken(AuthToken token, long expirySecondsFromNow)
    {
        _realm.Write(() =>
        {
            token.ExpiryDate = DateTimeOffset.UtcNow.AddSeconds(expirySecondsFromNow);
        });
    }

    public void RemoveToken(AuthToken token)
    {
        _realm.Write(() =>
        {
            _realm.Remove(token);
        });
    }

    public void RemoveToken(IQueryable<AuthToken> token)
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
    
    public AuthToken? GetTokenWithId(string tokenId)
    {
        AuthToken? token = _realm.All<AuthToken>()
            .FirstOrDefault(s => s.Id == tokenId);
        
        return token;
    }
}