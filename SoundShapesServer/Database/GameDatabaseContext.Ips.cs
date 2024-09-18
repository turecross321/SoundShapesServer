using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbIp GetOrCreateIp(DbUser user, string ipAddress)
    {
        DbIp? existingIp = Ips.Include(i => i.User)
            .FirstOrDefault(i => i.UserId == user.Id && i.IpAddress == ipAddress);

        if (existingIp != null)
            return existingIp;

        EntityEntry<DbIp> ip = Ips.Add(new DbIp
        {
            IpAddress = ipAddress,
            CreationDate = Time.Now,
            UserId = user.Id
        });

        SaveChanges();

        return ip.Entity;
    }
}