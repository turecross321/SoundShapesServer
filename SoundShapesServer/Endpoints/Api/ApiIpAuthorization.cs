using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.IP_Authorization;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Users;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api;

public class ApiIpAuthorization : EndpointGroup
{
    [ApiEndpoint("ip/authorize", Method.Post)]
    public Response AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthorizeIpRequest body, GameUser user)
    {
        IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress, (int)SessionType.Game);

        if (database.AuthorizeIpAddress(ip, body.OneTimeUse))
            return HttpStatusCode.Created;

        return HttpStatusCode.Conflict;
    }
    [ApiEndpoint("ip/unAuthorize", Method.Post)]
    public Response UnAuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiUnAuthorizeIpRequest body, GameUser user)
    {
        IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress, (int)SessionType.Game);

        database.RemoveIpAddress(ip);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("ip/addresses")]
    public ApiIpAddressesWrapper GetAddresses(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool? authorized = null;
        if (TryParse(context.QueryString["authorized"], out bool authorizedTemp)) authorized = authorizedTemp;
        
        (IpAuthorization[] addresses, int totalAddresses) =
            database.GetIpAddresses(user, from, count,  SessionType.Game, authorized);

        return new ApiIpAddressesWrapper(addresses, totalAddresses);
    }
}