using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public (Punishment[], int) GetPunishments(bool descending, PunishmentFilters filters, int from, int count)
    {
        IQueryable<Punishment> orderedPunishments = PunishmentsOrderedByDate(descending);
        IQueryable<Punishment> filteredPunishments = FilterPunishments(orderedPunishments, filters);

        Punishment[] paginatedPunishments = PaginationHelper.PaginatePunishments(filteredPunishments, from, count);
        return (paginatedPunishments, filteredPunishments.Count());
    }
    
    private IQueryable<Punishment> FilterPunishments(IQueryable<Punishment> punishments, PunishmentFilters filters)
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

    private IQueryable<Punishment> PunishmentsOrderedByDate(bool descending)
    {
        if (descending) return _realm.All<Punishment>().OrderByDescending(p => p.IssuedAt);
        return _realm.All<Punishment>().OrderBy(p => p.IssuedAt);
    }

    public Punishment? GetPunishmentWithId(string id)
    {
        return _realm.All<Punishment>().FirstOrDefault(p => p.Id == id);
    }
    
    public Punishment PunishUser(GameUser issuer, GameUser recipient, ApiPunishRequest request)
    {
        if (request.PunishmentType == (int)PunishmentType.Ban)
        {
            RemoveAllSessionsWithUser(recipient);
        }
        
        Punishment newPunishment = new ()
        {
            Id = GenerateGuid(),
            PunishmentType = request.PunishmentType,
            Recipient = recipient,
            Reason = request.Reason,
            ExpiresAt = request.ExpiresAt,
            IssuedAt = DateTimeOffset.UtcNow,
            Author = issuer
        };
        
        _realm.Write(() =>
        {
            _realm.Add(newPunishment);
        });

        return newPunishment;
    }

    public Punishment EditPunishment(Punishment punishment, GameUser recipient, ApiPunishRequest request)
    {
        _realm.Write(() =>
        {
            punishment.Recipient = recipient;
            punishment.PunishmentType = request.PunishmentType;
            punishment.Reason = request.Reason;
            punishment.ExpiresAt = request.ExpiresAt;
        });

        return punishment;
    }

    public void RevokePunishment(Punishment punishment)
    {
        _realm.Write(() =>
        {
            punishment.Revoked = true;
        });
    }
}