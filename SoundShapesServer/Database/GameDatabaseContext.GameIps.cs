using SoundShapesServer.Extensions.Queryable;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public GameIp CreateGameIp(GameUser user, string ipAddress)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        
        GameIp gameIp = new()
        {
            IpAddress = ipAddress,
            User = user,
            CreationDate = now,
            ModificationDate = now
        };
        
        _realm.Write(() =>
        {
            _realm.Add(gameIp);
        });

        return gameIp;
    }

    public GameIp? GetIpWithAddress(GameUser user, string ipAddress)
    {
        return user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress);
    }
    public void AuthorizeIpAddress(GameIp gameIp, bool oneTime)
    {
        _realm.Write(() =>
        {
            gameIp.Authorized = true;
            gameIp.OneTimeUse = oneTime;
            gameIp.ModificationDate = DateTimeOffset.UtcNow;
        });

        _realm.Refresh();
    }

    public void RemoveIpAddress(GameIp gameIp)
    {
        _realm.Write(() =>
        {
            // Remove all tokens with ip address
            foreach (GameToken token in gameIp.Tokens)
            {
                _realm.Remove(token);
            }
            
            _realm.Remove(gameIp);
        });

        _realm.Refresh();
    }
    
    private void RemoveIpAddresses(IQueryable<GameIp> ips)
    {
        _realm.Write(() =>
        {
            // Not removing tokens here because that is handled by Tok
            _realm.RemoveRange(ips);
        });

        _realm.Refresh();
    }

    public void UseOneTimeIpAddress(GameIp gameIp)
    {
        _realm.Write(() =>
        {
            gameIp.Authorized = false;
            gameIp.OneTimeUse = false;
            gameIp.ModificationDate = DateTimeOffset.UtcNow;
        });
    }

    public PaginatedList<GameIp> GetPaginatedIps(GameUser user, bool? authorized, int from, int count)
    {
        return new PaginatedList<GameIp>(GetIps(user, authorized), from, count);
    }

    private IQueryable<GameIp> GetIps(GameUser user, bool? authorized)
    {
        return _realm.All<GameIp>().Where(i => i.User == user).FilterIpAddresses(authorized);
    }
}