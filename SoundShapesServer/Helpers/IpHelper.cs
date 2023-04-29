using System.Net;
using Bunkum.HttpServer;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.IP_Authorization;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class IpHelper
{
    public static IpAuthorization GetIpAuthorizationFromRequestContext(RequestContext context, RealmDatabaseContext database, GameUser user, SessionType sessionType)
    {
        string ipAddress = ((IPEndPoint)context.RemoteEndpoint).Address.ToString();
        IpAuthorization ip = database.GetIpFromAddress(user, ipAddress, sessionType);

        return ip;
    }

    public static ApiAuthorizedIpResponse IpAuthorizationToAuthorizedIpResponse(IpAuthorization ip)
    {
        return new ApiAuthorizedIpResponse()
        {
            IpAddress = ip.IpAddress,
            OneTimeUse = ip.OneTimeUse
        };
    }

    public static ApiUnAuthorizedIpResponse IpAuthorizationToUnAuthorizedIpResponse(IpAuthorization ip)
    {
        return new ApiUnAuthorizedIpResponse()
        {
            IpAddress = ip.IpAddress
        };
    }
}