using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class PunishmentHelper
{
    public static IQueryable<Punishment> GetActivePunishments(GameUser user)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return user.Punishments.Where(p=>p.ExpiryDate > now && !p.Revoked);
    }

    public static IQueryable<Punishment> GetActiveUserBans(GameUser user)
    {
        return GetActivePunishments(user)
            .Where(p => p._PunishmentType == (int)PunishmentType.Ban)
            .OrderByDescending(p=>p.ExpiryDate);
    }
}