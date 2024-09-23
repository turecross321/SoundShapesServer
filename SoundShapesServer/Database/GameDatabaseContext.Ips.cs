using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoundShapesServer.Types.Database;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public DbIp GetOrCreateIp(DbUser user, string ipAddress)
    {
        DbIp? existingIp = this.Ips.Include(i => i.User)
            .FirstOrDefault(i => i.UserId == user.Id && i.IpAddress == ipAddress);

        if (existingIp != null)
            return existingIp;

        EntityEntry<DbIp> ip = this.Ips.Add(new DbIp
        {
            IpAddress = ipAddress,
            CreationDate = this.Time.Now,
            UserId = user.Id
        });
        
        this.SaveChanges();

        return ip.Entity;
    }

    public IQueryable<DbIp> GetIpsWithUser(DbUser user)
    {
        return this.Ips.Where(i => i.UserId == user.Id);
    }
}