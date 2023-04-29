using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public void PunishUser(GameUser user, ApiPunishRequest request)
    {
        if (request.PunishmentType == (int)PunishmentType.Ban)
        {
            this.RemoveAllSessionsWithUser(user);
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
        
        this._realm.Write(() =>
        {
            this._realm.Add(newPunishment);
        });
    }
}