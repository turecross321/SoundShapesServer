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
        DbToken token = this.CreateToken(refresh.User, TokenType.ApiAccess, null, null, refresh, null);
        // update the refresh expiry date
        refresh.ExpiryDate = this._time.Now.AddHours(ExpiryTimes.RefreshTokenHours);
        this.SaveChanges();

        return token;
    }

    public DbRefreshToken? GetRefreshTokenWithId(Guid guid)
    {
        DbRefreshToken? token = this.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefault(t => t.Id == guid);

        if (this._time.Now >= token?.ExpiryDate)
        {
            this.RemoveRefreshToken(token);
            return null;
        }

        return token;
    }
    
    public void RemoveRefreshToken(DbRefreshToken token)
    {
        this.RefreshTokens.Remove(token);
        this.SaveChanges();
    }

    public DbRefreshToken CreateRefreshToken(DbUser user)
    {
        EntityEntry<DbRefreshToken> token = this.RefreshTokens.Add(new DbRefreshToken
        {
            UserId = user.Id,
            CreationDate = this._time.Now,
            ExpiryDate = this._time.Now.AddHours(ExpiryTimes.RefreshTokenHours),
        });
        
        this.SaveChanges();

        // Reload to load the ID
        token.Reload();

        return token.Entity;
    }
    
    public IQueryable<DbRefreshToken> GetRefreshTokens() => this.RefreshTokens;
}