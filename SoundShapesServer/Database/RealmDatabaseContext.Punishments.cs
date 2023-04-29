using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public void PunishUser(GameUser user, PunishRequest request)
    {
        if (request.PunishmentType == (int)PunishmentType.Ban)
        {
            this.RemoveAllSessionsWithUser(user);
        }
        
        this._realm.Write(() =>
        {
            Punishment newPunishment = new ()
            {
                PunishmentType = request.PunishmentType,
                User = user,
                Reason = request.Reason,
                ExpiresAt = request.ExpiresAtUtc,
                IssuedAt = DateTimeOffset.UtcNow
            };
            
            this._realm.Add(newPunishment);
        });
    }
}