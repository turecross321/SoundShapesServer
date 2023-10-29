using MongoDB.Bson;
using SoundShapesServer.Extensions.Queryable;
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
        if (!ObjectId.TryParse(id, out ObjectId objectId)) 
            return null;
        
        return _realm.All<Punishment>().FirstOrDefault(p => p.Id == objectId);
    }
    
    public (Punishment[], int) GetPaginatedPunishments(PunishmentOrderType order, bool descending, PunishmentFilters filters, int from, int count)
    {
        return _realm.All<Punishment>().FilterPunishments(filters).OrderPunishments(order, descending).Paginate(from, count);
    }
}