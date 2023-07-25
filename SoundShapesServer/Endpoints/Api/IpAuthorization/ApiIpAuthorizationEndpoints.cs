using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.IpAuthorization;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.IpAuthorization;

public class ApiIpAuthorizationEndpoints : EndpointGroup
{
    [ApiEndpoint("ip/authorize", Method.Post)]
    [DocSummary("Authorizes specified IP address.")]
    public Response AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthorizeIpRequest body, GameUser user)
    {
        Types.IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress);

        if (database.AuthorizeIpAddress(ip, body.OneTimeUse))
            return HttpStatusCode.Created;

        return HttpStatusCode.Conflict;
    }
    [ApiEndpoint("ip/address/{address}", Method.Delete)]
    [DocSummary("Deletes specified IP address.")]
    public Response UnAuthorizeIpAddress(RequestContext context, GameDatabaseContext database, string address, GameUser user)
    {
        Types.IpAuthorization ip = database.GetIpFromAddress(user, address);

        database.RemoveIpAddress(ip);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("ip")]
    [DocUsesPageData]
    [DocSummary("Lists user's IP addresses.")]
    [DocQueryParam("authorized", "Removes authorized/unauthorized IP addresses from result.")]
    public ApiIpAddressesWrapper GetAddresses(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);
        
        bool? authorized = null;
        if (bool.TryParse(context.QueryString["authorized"], out bool authorizedTemp)) authorized = authorizedTemp;
        
        (Types.IpAuthorization[] addresses, int totalAddresses) =
            database.GetIpAddresses(user, from, count, authorized);

        return new ApiIpAddressesWrapper(addresses, totalAddresses);
    }
}