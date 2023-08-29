using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private GameIp CreateIpAddress(GameUser user, string ipAddress)
    {
        GameIp gameIp = new(ipAddress, user);
        
        _realm.Write(() =>
        {
            _realm.Add(gameIp);
        });

        return gameIp;
    }

    public GameIp GetIpFromAddress(GameUser user, string ipAddress)
    {
        _realm.Refresh();
        
        GameIp? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress);
        return ip ?? CreateIpAddress(user, ipAddress);
    }
    public bool AuthorizeIpAddress(GameIp gameIp, bool oneTime)
    {
        if (gameIp.Authorized) return false;
        
        _realm.Write(() =>
        {
            gameIp.Authorized = true;
            gameIp.OneTimeUse = oneTime;
            gameIp.ModificationDate = DateTimeOffset.UtcNow;

            foreach (GameSession session in gameIp.Sessions.Where(s=>s._SessionType == (int)SessionType.GameUnAuthorized))
            {
                session.SessionType = SessionType.Game;
            }
        });

        _realm.Refresh();
        
        return true;
    }

    public void RemoveIpAddress(GameIp gameIp)
    {
        _realm.Write(() =>
        {
            // Remove all sessions with ip address
            foreach (GameSession session in gameIp.Sessions)
            {
                _realm.Remove(session);
            }
            
            _realm.Remove(gameIp);
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

    public (GameIp[], int) GetPaginatedIps(GameUser user, bool? authorized, int from, int count)
    {
        IQueryable<GameIp> filteredAddresses = GetIps(user, authorized);
        GameIp[] paginatedAddresses = PaginationHelper.PaginateIpAddresses(filteredAddresses, from, count);

        return (paginatedAddresses, filteredAddresses.Count());
    }

    private IQueryable<GameIp> GetIps(GameUser user, bool? authorized)
    {
        IQueryable<GameIp> addresses = _realm.All<GameIp>().Where(i => i.User == user);
        IQueryable<GameIp> filteredAddresses = FilterIpAddresses(addresses, authorized);

        return filteredAddresses;
    }

    private static IQueryable<GameIp> FilterIpAddresses(IQueryable<GameIp> addresses,
        bool? authorized)
    {
        IQueryable<GameIp> response = addresses;

        if (authorized != null)
        {
            response = response.Where(i => i.Authorized == authorized);
        }

        return response;
    }
}