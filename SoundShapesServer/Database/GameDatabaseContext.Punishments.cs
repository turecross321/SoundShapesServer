using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    // TODO: Implement same ordering system as levels
    public IQueryable<Punishment> GetPunishments()
    {
        return _realm.All<Punishment>();
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
            User = recipient,
            Reason = request.Reason,
            ExpiresAt = request.ExpiresAtUtc,
            IssuedAt = DateTimeOffset.UtcNow,
            Issuer = issuer
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
            punishment.User = recipient;
            punishment.PunishmentType = request.PunishmentType;
            punishment.Reason = request.Reason;
            punishment.ExpiresAt = request.ExpiresAtUtc;
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