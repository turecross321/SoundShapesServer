using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Extensions;

public static class PunishmentQueryableExtensions
{
    public static IQueryable<Punishment> ActivePunishments(this IQueryable<Punishment> punishments)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        return punishments.Where(p=>p.ExpiryDate > now && !p.Revoked);
    }

    public static IQueryable<Punishment> ActiveBans(this IQueryable<Punishment> punishments)
    {
        return punishments
            .ActivePunishments()
            .Where(p => p._PunishmentType == (int)PunishmentType.Ban)
            .OrderByDescending(p=>p.ExpiryDate);
    }
}