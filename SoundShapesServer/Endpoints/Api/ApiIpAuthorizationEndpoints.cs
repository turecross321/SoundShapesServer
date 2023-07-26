using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api;

public class ApiIpAuthorizationEndpoints : EndpointGroup
{
    [ApiEndpoint("ip/authorize", Method.Post)]
    [DocSummary("Authorizes specified IP address.")]
    [DocError(typeof(ConflictError), ConflictError.AlreadyAuthorizedIpWhen)]
    public Response AuthorizeIpAddress(RequestContext context, GameDatabaseContext database, ApiAuthorizeIpRequest body, GameUser user)
    {
        Types.IpAuthorization ip = database.GetIpFromAddress(user, body.IpAddress);

        if (!database.AuthorizeIpAddress(ip, body.OneTimeUse))
            return new Response(ConflictError.AlreadyAuthorizedIpWhen, ContentType.Plaintext, HttpStatusCode.Conflict);
        
        return HttpStatusCode.Created;
    }
    [ApiEndpoint("ip/address/{address}", Method.Delete)]
    [DocSummary("Deletes specified IP address.")]
    public Response UnAuthorizeIpAddress(RequestContext context, GameDatabaseContext database, string address, GameUser user)
    {
        Types.IpAuthorization ip = database.GetIpFromAddress(user, address);

        database.RemoveIpAddress(ip);
        return HttpStatusCode.NoContent;
    }

    [ApiEndpoint("ip")]
    [DocUsesPageData]
    [DocSummary("Lists user's IP addresses.")]
    [DocQueryParam("authorized", "Filters authorized/unauthorized IP addresses from result.")]
    public ApiListResponse<ApiIpResponse> GetAddresses(RequestContext context, GameDatabaseContext database, GameUser user)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);
        
        bool? authorized = null;
        if (bool.TryParse(context.QueryString["authorized"], out bool authorizedTemp)) authorized = authorizedTemp;
        
        (Types.IpAuthorization[] addresses, int totalAddresses) =
            database.GetPaginatedIps(user, authorized, from, count);

        return new ApiListResponse<ApiIpResponse>(addresses.Select(a=>new ApiIpResponse(a)), totalAddresses);
    }
}