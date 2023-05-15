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

    public static Punishment? IsUserBanned(GameUser user)
    {
        return user.Punishments.AsEnumerable()
            .Where(p=>p is { PunishmentType: (int)PunishmentType.Ban, Revoked: false })
            .MaxBy(p => p.ExpiresAt);
    }
}