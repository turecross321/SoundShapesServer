using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Workers;

public class ExpiredObjectWorker : IWorker
{
    public int WorkIntervalMilliseconds => 1000 * 60; // 1 minute
    public void DoWork(DataContext context)
    {
        DateTime now = context.Database.Time.Now;
        
        IQueryable<DbUser> expiredUsers = 
            context.Database.GetUsers().Where(u => !u.FinishedRegistration && (u.RegistrationExpiryDate < now || u.RegistrationExpiryDate == null));
        context.Logger.LogInfo(SSSContext.Worker, $"Removed {expiredUsers.Count()} expired users.");
        context.Database.RemoveRange(expiredUsers);
        
        IQueryable<DbRefreshToken> expiredRefreshTokens = context.Database.GetRefreshTokens().Where(t => t.ExpiryDate < now);
        context.Logger.LogInfo(SSSContext.Worker, $"Removed {expiredRefreshTokens.Count()} expired refresh tokens.");
        context.Database.RemoveRange(expiredRefreshTokens);
        
        IQueryable<DbToken> expiredTokens = context.Database.GetTokens().Where(t => t.ExpiryDate < now);
        context.Logger.LogInfo(SSSContext.Worker, $"Removed {expiredTokens.Count()} expired tokens.");
        context.Database.RemoveRange(expiredTokens);
        
        IQueryable<DbCode> expiredCodes = context.Database.GetCodes().Where(c => c.ExpiryDate < now);
        context.Logger.LogInfo(SSSContext.Worker, $"Removed {expiredCodes.Count()} expired tokens.");
        context.Database.RemoveRange(expiredCodes);
        
        context.Database.SaveChanges();
    }
}