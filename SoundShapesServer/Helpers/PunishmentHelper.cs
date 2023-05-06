using SoundShapesServer.Types;

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

    public static IQueryable<Punishment> OrderPunishments(IQueryable<Punishment> punishments, bool descending)
    {
        return descending ? punishments.AsEnumerable().Reverse().AsQueryable() : punishments;
    }
}