using SoundShapesServer.Authentication;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.IpHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IpAuthorization AddUserIp(GameUser user, string ipAddress, TypeOfSession sessionType)
    {
        IpAuthorization ip = new IpAuthorization() { IpAddress = ipAddress, User = user, SessionType = (int)sessionType};
        
        this._realm.Write(() =>
        {
            this._realm.Add(ip);
        });

        return ip;
    }

    public IpAuthorization GetIpFromAddress(GameUser user, string ipAddress, TypeOfSession sessionType)
    {
        IpAuthorization? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress && i.SessionType == (int)sessionType);
        if (ip == null) ip = AddUserIp(user, ipAddress, sessionType);
        
        return ip;
    }
    public bool AuthorizeIpAddress(IpAuthorization ip, bool oneTime)
    {
        if (ip.Authorized) return false;
        
        this._realm.Write(() =>
        {
            ip.Authorized = true;
            ip.OneTimeUse = oneTime;
        });
        
        this._realm.Refresh();

        return true;
    }

    public void RemoveIpAddress(IpAuthorization ip)
    {
        GameSession[] sessionsFromIp = GetSessionsWithIp(ip);
        
        this._realm.Write(() =>
        {
            // Remove all sessions with ip address
            foreach (var session in sessionsFromIp)
            {
                this._realm.Remove(session);
            }
            
            this._realm.Remove(ip);
        });

        this._realm.Refresh();
    }

    public void UseIpAddress(IpAuthorization ip)
    {
        this._realm.Write(() =>
        {
            ip.Authorized = false;
            ip.OneTimeUse = false;
        });
    }

    public ApiUnAuthorizedIpResponse[] GetUnAuthorizedIps(GameUser user, TypeOfSession sessionType)
    {
        List<string> addresses = new List<string>();
        
        IpAuthorization[] unAuthorizedIps = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized == false && i.SessionType == (int)sessionType).ToArray();

        // Convert list to response array

        ApiUnAuthorizedIpResponse[] response = new ApiUnAuthorizedIpResponse[unAuthorizedIps.Length];
        
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = IpAuthorizationToUnAuthorizedIpResponse(unAuthorizedIps[i]);
        }

        return response;
    }

    public ApiAuthorizedIpResponse[] GetAuthorizedIps(GameUser user, TypeOfSession sessionType)
    {
        // Get currently authorized IPs
        List<IpAuthorization> authorizedIps = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized && i.SessionType == (int)sessionType).ToList();

        // Convert list to response array

        ApiAuthorizedIpResponse[] response = new ApiAuthorizedIpResponse[authorizedIps.Count];
        
        for (int i = 0; i < response.Length; i++)
        {
            response[i] = IpAuthorizationToAuthorizedIpResponse(authorizedIps[i]);
        }

        return response;
    }
}