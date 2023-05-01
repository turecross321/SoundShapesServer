using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<Punishment> GetPunishments()
    {
        return _realm.All<Punishment>();
    }

    public Punishment? GetPunishmentWithId(string id)
    {
        return _realm.All<Punishment>().FirstOrDefault(p => p.Id == id);
    }
    
    public Punishment PunishUser(GameUser user, ApiPunishRequest request)
    {
        if (request.PunishmentType == (int)PunishmentType.Ban)
        {
            RemoveAllSessionsWithUser(user);
        }
        
        Punishment newPunishment = new ()
        {
            Id = GenerateGuid(),
            PunishmentType = request.PunishmentType,
            User = user,
            Reason = request.Reason,
            ExpiresAt = request.ExpiresAtUtc,
            IssuedAt = DateTimeOffset.UtcNow
        };
        
        _realm.Write(() =>
        {
            _realm.Add(newPunishment);
        });

        return newPunishment;
    }

    public void EditPunishment(Punishment punishment, ApiPunishRequest request, GameUser user)
    {
        _realm.Write(() =>
        {
            punishment.User = user;
            punishment.PunishmentType = request.PunishmentType;
            punishment.Reason = request.Reason;
            punishment.ExpiresAt = request.ExpiresAtUtc;
        });
    }

    public void DismissPunishment(Punishment punishment)
    {
        _realm.Write(() =>
        {
            punishment.Dismissed = true;
        });
    }
}