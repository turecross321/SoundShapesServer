using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private IpAuthorization CreateIpAddress(GameUser user, string ipAddress)
    {
        IpAuthorization ip = new(ipAddress, user);
        
        _realm.Write(() =>
        {
            _realm.Add(ip);
        });

        return ip;
    }

    public IpAuthorization GetIpFromAddress(GameUser user, string ipAddress)
    {
        _realm.Refresh();
        
        IpAuthorization? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress);
        return ip ?? CreateIpAddress(user, ipAddress);
    }
    public bool AuthorizeIpAddress(IpAuthorization ip, bool oneTime)
    {
        if (ip.Authorized) return false;
        
        _realm.Write(() =>
        {
            ip.Authorized = true;
            ip.OneTimeUse = oneTime;
            ip.ModificationDate = DateTimeOffset.UtcNow;

            foreach (GameSession session in ip.Sessions.Where(s=>s._SessionType == (int)SessionType.GameUnAuthorized))
            {
                session.SessionType = SessionType.Game;
            }
        });

        _realm.Refresh();
        
        return true;
    }

    public void RemoveIpAddress(IpAuthorization ip)
    {
        IEnumerable<GameSession> sessionsFromIp = GetSessionsWithIp(ip);
        
        _realm.Write(() =>
        {
            // Remove all sessions with ip address
            foreach (var session in sessionsFromIp)
            {
                _realm.Remove(session);
            }
            
            _realm.Remove(ip);
        });

        _realm.Refresh();
    }

    public void UseOneTimeIpAddress(IpAuthorization ip)
    {
        _realm.Write(() =>
        {
            ip.Authorized = false;
            ip.OneTimeUse = false;
            ip.ModificationDate = DateTimeOffset.UtcNow;
        });
    }

    public (IpAuthorization[], int) GetPaginatedIps(GameUser user, bool? authorized, int from, int count)
    {
        IQueryable<IpAuthorization> filteredAddresses = GetIps(user, authorized);
        IpAuthorization[] paginatedAddresses = PaginationHelper.PaginateIpAddresses(filteredAddresses, from, count);

        return (paginatedAddresses, filteredAddresses.Count());
    }

    private IQueryable<IpAuthorization> GetIps(GameUser user, bool? authorized)
    {
        IQueryable<IpAuthorization> addresses = _realm.All<IpAuthorization>().Where(i => i.User == user);
        IQueryable<IpAuthorization> filteredAddresses = FilterIpAddresses(addresses, authorized);

        return filteredAddresses;
    }

    private static IQueryable<IpAuthorization> FilterIpAddresses(IQueryable<IpAuthorization> addresses,
        bool? authorized)
    {
        IQueryable<IpAuthorization> response = addresses;

        if (authorized != null)
        {
            response = response.Where(i => i.Authorized == authorized);
        }

        return response;
    }
}