using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiIpAuthorization : EndpointGroup
{
    [ApiEndpoint("ip/authorize", Method.Post)]
    public Response AuthorizeIpAddress(RequestContext context, RealmDatabaseContext database, ApiAuthenticateIpRequest body, GameUser user)
    {
        IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress);

        if (database.AuthorizeIpAddress(ip, body.OneTimeUse))
            return HttpStatusCode.Created;

        return HttpStatusCode.Conflict;
    }
    [ApiEndpoint("ip/unAuthorize", Method.Post)]
    public Response UnAuthorizeIpAddress(RequestContext context, RealmDatabaseContext database, ApiAuthenticateIpRequest body, GameUser user)
    {
        IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress);

        database.RemoveIpAddress(ip);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("ip/unAuthorized", Method.Get)]
    public ApiIpAuthorizations UnAuthorizedIps(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        string[] addresses = database.GetUnAuthorizedIps(user);
        
        return new ApiIpAuthorizations()
        {
            IpAddresses = addresses
        };
    }

    [ApiEndpoint("ip/authorized")]
    public ApiIpAuthorizations AuthorizedIps(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        string[] addresses = database.GetAuthorizedIps(user);
        
        return new ApiIpAuthorizations()
        {
            IpAddresses = addresses
        };
    }
}