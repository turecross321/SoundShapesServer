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

    public static Punishment? IsUserBanned(GameUser user)
    {
        Punishment[] bans = GetUsersPunishmentsOfType(user, PunishmentType.Ban);
        return bans.Length > 0 ? bans.AsEnumerable().OrderByDescending(p => p.ExpiresAt).First() : null;
    }

    public static IQueryable<Punishment> FilterPunishments(IQueryable<Punishment> punishments, string? byUser, string? forUser, bool? revoked)
    {
        IQueryable<Punishment> response = punishments;

        if (byUser != null)
        {
            response = response.Where(p => p.Issuer.Id == byUser);
        }

        if (forUser != null)
        {
            response = response.Where(p => p.User.Id == forUser);
        }

        if (revoked != null)
        {
            response = response.Where(p => p.Revoked == revoked);
        }

        return response;
    }
}