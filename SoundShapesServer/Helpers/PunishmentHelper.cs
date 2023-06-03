using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class PunishmentHelper
{
    public static IQueryable<Punishment> GetActivePunishments(GameUser user)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return user.Punishments.AsEnumerable().Where(p=>p.ExpiresAt > now && !p.Revoked).AsQueryable();
    }

    public static IQueryable<Punishment> GetActiveUserBans(GameUser user)
    {
        DateTimeOffset utcNow = DateTimeOffset.UtcNow;
        
        return user.Punishments
            .Where(p=> p.PunishmentType == (int)PunishmentType.Ban && !p.Revoked && p.ExpiresAt > utcNow)
            .OrderByDescending(p=>p.ExpiresAt);
    }
}