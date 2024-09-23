using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbToken? GetTokenWithId(Guid guid)
    {
        return Tokens
            .Include(t => t.User)
            .Include(t => t.RefreshToken)
            .FirstOrDefault(t => t.Id == guid);
    }
    
    public DbToken CreateToken(DbUser user, TokenType tokenType, PlatformType? platformType, DbIp? ip, 
        DbRefreshToken? refreshToken, bool? genuineNpTicket)
    {
        int expiryHours = tokenType switch
        {
            TokenType.GameAccess => ExpiryTimes.GameTokenHours,
            TokenType.GameEula => ExpiryTimes.CodeHours,
            TokenType.ApiAccess => ExpiryTimes.ApiAccessHours,
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };

        DateTimeOffset expiry = _time.Now.AddHours(expiryHours);

        // If there is a refresh token and it expires before this would normally expire, use the refresh expiry date instead.
        // This is to prevent a situation where the refresh token is expired but there are still tokens
        // generated with it that are usable.
        if (refreshToken != null && refreshToken.ExpiryDate < expiry)
            expiry = refreshToken.ExpiryDate;
        
        EntityEntry<DbToken> token = Tokens.Add(new DbToken
        {
            UserId = user.Id,
            TokenType = tokenType,
            CreationDate = _time.Now,
            ExpiryDate = expiry,
            Platform = platformType,
            IpId = ip?.Id,
            RefreshTokenId = refreshToken?.Id,
            GenuineNpTicket = genuineNpTicket,
        });

        SaveChanges();
        
        // Reload to load the ID
        token.Reload();

        return token.Entity;
    }
    
    public IQueryable<DbToken> GetTokens() => Tokens;
    
    public void RemoveToken(DbToken token)
    {
        Tokens.Remove(token);
        SaveChanges();
    }
}