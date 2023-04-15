using SoundShapesServer.Authentication;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.IpHelper;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IpAuthorization AddUserIp(GameUser user, string ipAddress)
    {
        if (IsIpAlreadyAdded(user, ipAddress))
            return GetIpFromAddress(user, ipAddress);
        
        IpAuthorization ip = new IpAuthorization() { IpAddress = ipAddress, User = user};
        
        this._realm.Write(() =>
        {
            this._realm.Add(ip);
        });

        return ip;
    }

    public IpAuthorization GetIpFromAddress(GameUser user, string ipAddress)
    {
        IpAuthorization? ip = user.IpAddresses.FirstOrDefault(i => i.IpAddress == ipAddress);
        if (ip == null) ip = AddUserIp(user, ipAddress);
        
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
    }

    public void UseOneTimeIpAddress(IpAuthorization ip)
    {
        this._realm.Write(() =>
        {
            ip.Authorized = false;
            ip.OneTimeUse = false;
        });
    }

    public string[] GetUnAuthorizedIps(GameUser user)
    {
        List<string> addresses = new List<string>();
        
        IpAuthorization[] unauthorizedIpAddresses = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized == false).ToArray();

        for (int i = 0; i < unauthorizedIpAddresses.Length; i++)
        {
            addresses.Add(unauthorizedIpAddresses[i].IpAddress);
        }

        return addresses.ToArray();
    }

    public string[] GetAuthorizedIps(GameUser user)
    {
        List<string> authorizedIpAddresses = new List<string>();
        
        // Get currently authorized IPs
        List<IpAuthorization> authorizedIps = user.IpAddresses.AsEnumerable().Where(i=>i.Authorized).ToList();
        
        for (int i = 0; i < authorizedIps.Count; i++)
        {
            authorizedIpAddresses.Add(authorizedIps[i].IpAddress);
        }
        
        // Get IPs with game session (this is to get the one time sessions that technically are unauthorized, but currently have a session)
        List<string> ipsWithSession = new List<string>();
        GameSession[] sessions = user.Sessions.AsEnumerable().Where(s=> GameSessionTypes.Contains(s.SessionType)).ToArray();

        for (int i = 0; i < sessions.Length; i++)
        {
            ipsWithSession.Add(sessions[i].Ip.IpAddress);   
        }

        return authorizedIpAddresses.Union(ipsWithSession).ToArray();
    }
}