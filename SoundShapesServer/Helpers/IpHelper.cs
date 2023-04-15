using System.Net;
using Bunkum.HttpServer;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class IpHelper
{
    public static bool IsIpAuthorized(GameUser user, string ipAddress)
    {
        IpAuthorization[] authorizedIps = user.IpAddresses.AsEnumerable().Where(i => i.Authorized).ToArray();
        string[] authorizedIpAddresses = new string[authorizedIps.Length];

        for (int i = 0; i < authorizedIps.Length; i++)
        {
            authorizedIpAddresses[i] = authorizedIps[i].IpAddress;
        }

        return authorizedIpAddresses.Contains(ipAddress);
    }
    
    public static bool IsIpAlreadyAdded(GameUser user, string ipAddress)
    {
        IpAuthorization[] authorizedIps = user.IpAddresses.AsEnumerable().ToArray();
        string[] ipAddresses = new string[authorizedIps.Length];

        for (int i = 0; i < authorizedIps.Length; i++)
        {
            ipAddresses[i] = authorizedIps[i].IpAddress;
        }

        return ipAddresses.Contains(ipAddress);
    }

    public static IpAuthorization GetIpAuthorizationFromRequestContext(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        string ipAddress = ((IPEndPoint)context.RemoteEndpoint).Address.ToString();
        IpAuthorization ip = database.GetIpFromAddress(user, ipAddress) ?? database.AddUserIp(user, ipAddress);

        return ip;
    }
}