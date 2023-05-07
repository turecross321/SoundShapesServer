using SoundShapesServer.Authentication;
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

    public ApiUnAuthorizedIpResponse[] GetUnAuthorizedIps(GameUser user, SessionType sessionType)
    {
        IpAuthorization[] unAuthorizedIps = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized == false && i.SessionType == (int)sessionType).ToArray();

        // Convert list to response array

        ApiUnAuthorizedIpResponse[] response = new ApiUnAuthorizedIpResponse[unAuthorizedIps.Length];
        
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = new ApiUnAuthorizedIpResponse(unAuthorizedIps[i]);
        }

        return response;
    }

    public ApiAuthorizedIpResponse[] GetAuthorizedIps(GameUser user, SessionType sessionType)
    {
        // Get currently authorized IPs
        List<IpAuthorization> authorizedIps = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized && i.SessionType == (int)sessionType).ToList();

        // Convert list to response array

        ApiAuthorizedIpResponse[] response = new ApiAuthorizedIpResponse[authorizedIps.Count];
        
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = new ApiAuthorizedIpResponse(authorizedIps[i]);
        }

        return response;
    }
}