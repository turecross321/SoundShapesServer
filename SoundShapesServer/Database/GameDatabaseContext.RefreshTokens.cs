using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbToken CreateApiTokenWithRefreshToken(DbRefreshToken refresh)
    {
        DbToken token = CreateToken(refresh.User, TokenType.ApiAccess, null, null, refresh, null);
        // update the refresh expiry date
        refresh.ExpiryDate = _time.Now.AddHours(ExpiryTimes.RefreshTokenHours);
        SaveChanges();

        return token;
    }

    public DbRefreshToken? GetRefreshTokenWithId(Guid guid)
    {
        DbRefreshToken? token = RefreshTokens
            .Include(t => t.User)
            .FirstOrDefault(t => t.Id == guid);

        if (_time.Now >= token?.ExpiryDate)
        {
            RemoveRefreshToken(token);
            return null;
        }

        return token;
    }

    public void RemoveRefreshToken(DbRefreshToken token)
    {
        RefreshTokens.Remove(token);

        SaveChanges();
    }

    public DbRefreshToken CreateRefreshToken(DbUser user)
    {
        EntityEntry<DbRefreshToken> token = RefreshTokens.Add(new DbRefreshToken
        {
            UserId = user.Id,
            CreationDate = _time.Now,
            ExpiryDate = _time.Now.AddHours(ExpiryTimes.RefreshTokenHours)
        });

        SaveChanges();

        // Reload to load the ID
        token.Reload();

        return token.Entity;
    }
}