using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<Punishment> GetPunishments()
    {
        return _realm.All<Punishment>();
    }
    
    public void PunishUser(GameUser user, ApiPunishRequest request)
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
    }
}