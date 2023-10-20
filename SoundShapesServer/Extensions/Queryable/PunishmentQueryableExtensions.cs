using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Extensions.Queryable;

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
    
    public static IQueryable<Punishment> FilterPunishments(this IQueryable<Punishment> punishments, PunishmentFilters filters)
    {
        if (filters.Author != null)
        {
            punishments = punishments.Where(p => p.Author == filters.Author);
        }

        if (filters.Recipient != null)
        {
            punishments = punishments.Where(p => p.Recipient == filters.Recipient);
        }

        if (filters.Revoked != null)
        {
            punishments = punishments.Where(p => p.Revoked == filters.Revoked);
        }

        return punishments;
    }

    public static IQueryable<Punishment> OrderPunishments(this IQueryable<Punishment> punishments, PunishmentOrderType order,
        bool descending)
    {
        return order switch
        {
            PunishmentOrderType.CreationDate => punishments.OrderByDynamic(p => p.CreationDate, descending),
            _ => punishments.OrderPunishments(PunishmentOrderType.CreationDate, descending)
        };
    }
}