using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class PunishmentHelper
{
    public static IQueryable<Punishment> GetActivePunishments(GameUser user)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return user.Punishments.AsEnumerable().Where(p=>p.ExpiresAt > now && !p.Revoked).AsQueryable();
    }
    
    public static Punishment[] GetUsersPunishmentsOfType(GameUser user, PunishmentType type)
    {
        return GetActivePunishments(user).AsEnumerable()
            .Where(p => p.PunishmentType == (int)type).ToArray();
    }
}