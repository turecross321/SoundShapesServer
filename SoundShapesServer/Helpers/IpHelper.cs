using System.Net;
using Bunkum.HttpServer;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Helpers;

public static class IpHelper
{
    public static IpAuthorization GetIpAuthorizationFromRequestContext(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        string ipAddress = ((IPEndPoint)context.RemoteEndpoint).Address.ToString();
        IpAuthorization ip = database.GetIpFromAddress(user, ipAddress);

        return ip;
    }
}