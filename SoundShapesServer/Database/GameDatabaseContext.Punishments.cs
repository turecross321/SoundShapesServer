using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public Punishment CreatePunishment(GameUser author, GameUser recipient, ApiPunishRequest request)
    {
        if (request.PunishmentType == PunishmentType.Ban)
        {
            RemoveAllSessionsWithUser(recipient);
        }
        
        Punishment newPunishment = new()
        {
            Id = GenerateGuid(),
            PunishmentType = request.PunishmentType,
            Recipient = recipient,
            Reason = request.Reason,
            ExpiryDate = DateTimeOffset.FromUnixTimeSeconds(request.ExpiryDate),
            CreationDate = DateTimeOffset.UtcNow,
            Author = author
        };
        
        _realm.Write(() =>
        {
            _realm.Add(newPunishment);
        });

        return newPunishment;
    }

    public Punishment EditPunishment(GameUser author, Punishment punishment, GameUser recipient, ApiPunishRequest request)
    {
        _realm.Write(() =>
        {
            punishment.Author = author;
            punishment.Recipient = recipient;
            punishment.PunishmentType = request.PunishmentType;
            punishment.Reason = request.Reason;
            punishment.ExpiryDate = DateTimeOffset.FromUnixTimeSeconds(request.ExpiryDate);
            punishment.ModificationDate = DateTimeOffset.UtcNow;
        });

        return punishment;
    }

    public void RevokePunishment(Punishment punishment)
    {
        _realm.Write(() =>
        {
            punishment.Revoked = true;
            punishment.RevokeDate = DateTimeOffset.UtcNow;
        });
    }
    
    public Punishment? GetPunishmentWithId(string id)
    {
        return _realm.All<Punishment>().FirstOrDefault(p => p.Id == id);
    }
    
    public (Punishment[], int) GetPunishments(PunishmentOrderType order, bool descending, PunishmentFilters filters, int from, int count)
    {
        IQueryable<Punishment> punishments = _realm.All<Punishment>();
        
        IQueryable<Punishment> filteredPunishments = FilterPunishments(punishments, filters);
        IQueryable<Punishment> orderedPunishments = OrderPunishments(filteredPunishments, order, descending);

        Punishment[] paginatedPunishments = PaginationHelper.PaginatePunishments(orderedPunishments, from, count);
        return (paginatedPunishments, filteredPunishments.Count());
    }
    
    private static IQueryable<Punishment> FilterPunishments(IQueryable<Punishment> punishments, PunishmentFilters filters)
    {
        IQueryable<Punishment> response = punishments;

        if (filters.Author != null)
        {
            response = response.Where(p => p.Author == filters.Author);
        }

        if (filters.Recipient != null)
        {
            response = response.Where(p => p.Recipient == filters.Recipient);
        }

        if (filters.Revoked != null)
        {
            response = response.Where(p => p.Revoked == filters.Revoked);
        }

        return response;
    }

    private static IQueryable<Punishment> OrderPunishments(IQueryable<Punishment> punishments, PunishmentOrderType order,
        bool descending)
    {
        return order switch
        {
            PunishmentOrderType.CreationDate => OrderPunishmentsByCreationDate(punishments, descending),
            _ => punishments
        };
    }
    
    private static IQueryable<Punishment> OrderPunishmentsByCreationDate(IQueryable<Punishment> punishments, bool descending)
    {
        if (descending) return punishments.OrderByDescending(p => p.CreationDate);
        return punishments.OrderBy(p => p.CreationDate);
    }
}