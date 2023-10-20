using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public Punishment CreatePunishment(GameUser author, GameUser recipient, ApiPunishRequest request)
    {
        Punishment newPunishment = new()
        {
            Id = GenerateGuid(),
            PunishmentType = request.PunishmentType,
            Recipient = recipient,
            Reason = request.Reason,
            ExpiryDate = DateTimeOffset.FromUnixTimeSeconds(request.ExpiryDate),
            CreationDate = DateTimeOffset.UtcNow,
            ModificationDate = DateTimeOffset.UtcNow,
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
    
    public (Punishment[], int) GetPaginatedPunishments(PunishmentOrderType order, bool descending, PunishmentFilters filters, int from, int count)
    {
        IQueryable<Punishment> punishments =
            _realm.All<Punishment>().FilterPunishments(filters).OrderPunishments(order, descending);
        return (punishments.Paginate(from, count), punishments.Count());
    }
}