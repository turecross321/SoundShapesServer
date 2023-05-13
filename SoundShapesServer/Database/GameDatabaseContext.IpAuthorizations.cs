using SoundShapesServer.Authentication;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.IP_Authorization;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    private IpAuthorization AddUserIp(GameUser user, string ipAddress, SessionType sessionType)
    {
        IpAuthorization ip = new() { IpAddress = ipAddress, User = user, SessionType = (int)sessionType};
        
        _realm.Write(() =>
        {
            _realm.Add(ip);
        });

        return ip;
    }

    public IpAuthorization GetIpFromAddress(GameUser user, string ipAddress, SessionType sessionType)
    {
        _realm.Refresh();
        
        IpAuthorization? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress && i.SessionType == (int)sessionType);
        if (ip == null) ip = AddUserIp(user, ipAddress, sessionType);
        
        return ip;
    }
    public bool AuthorizeIpAddress(IpAuthorization ip, bool oneTime)
    {
        if (ip.Authorized) return false;
        
        _realm.Write(() =>
        {
            ip.Authorized = true;
            ip.OneTimeUse = oneTime;
        });

        _realm.Refresh();
        
        return true;
    }

    public void RemoveIpAddress(IpAuthorization ip)
    {
        GameSession[] sessionsFromIp = GetSessionsWithIp(ip);
        
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

    public void UseIpAddress(IpAuthorization ip)
    {
        _realm.Write(() =>
        {
            ip.Authorized = false;
            ip.OneTimeUse = false;
        });
    }

    public (IpAuthorization[], int) GetIpAddresses(GameUser user, int from, int count, SessionType sessionType, bool? authorized)
    {
        IQueryable<IpAuthorization> addresses = _realm.All<IpAuthorization>().Where(i => i.User == user);

        IQueryable<IpAuthorization> filteredAddresses = FilterIpAddresses(addresses, sessionType, authorized);
        IpAuthorization[] paginatedAddresses = PaginationHelper.PaginateIpAddresses(filteredAddresses, from, count);

        return (paginatedAddresses, filteredAddresses.Count());
    }

    private IQueryable<IpAuthorization> FilterIpAddresses(IQueryable<IpAuthorization> addresses,
        SessionType sessionType, bool? authorized)
    {
        IQueryable<IpAuthorization> response = addresses;

        response = response.Where(i => i.SessionType == (int)sessionType);

        if (authorized != null)
        {
            response = response.Where(i => i.Authorized == authorized);
        }

        return response;
    }
}