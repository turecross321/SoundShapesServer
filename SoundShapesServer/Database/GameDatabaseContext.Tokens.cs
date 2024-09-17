using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Common.Types;
using SoundShapesServer.Common.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbToken? GetTokenWithId(Guid guid)
    {
        return Tokens.Include(t => t.User).FirstOrDefault(t => t.Id == guid);
    }
    
    public DbToken CreateToken(DbUser user, TokenType tokenType, PlatformType? platformType)
    {
        int expiryHours = tokenType switch
        {
            TokenType.GameAccess => ExpiryTimes.GameTokenHours,
            TokenType.GameEula => ExpiryTimes.CodeHours,
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };

        EntityEntry<DbToken> token = Tokens.Add(new DbToken
        {
            UserId = user.Id,
            TokenType = tokenType,
            CreationDate = _time.Now,
            ExpiryDate = _time.Now.AddHours(expiryHours),
            Platform = platformType,
        });

        SaveChanges();
        
        // Reload to load the ID
        token.Reload();

        return token.Entity;
    }

    public void RemoveToken(DbToken token)
    {
        Tokens.Remove(token);
        SaveChanges();
    }
}