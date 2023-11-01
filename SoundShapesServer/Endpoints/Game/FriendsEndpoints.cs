using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class FriendsEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{id}/~friends.all")]
    public Response GetFriends(RequestContext context)
    {
        return HttpStatusCode.OK;
    }

    [HttpEndpoint("/identity/person/{id}/data/psn/friends-list", HttpMethods.Post)]
    [GameEndpoint("identity/person/{id}/data/psn/friends-list", HttpMethods.Post)]
    [AllowEmptyBody]
    public Response UploadFriends(RequestContext context, GameDatabaseContext database, string? body, string id,
        GameUser user)
    {
        return new Response(HttpStatusCode.OK);
    }
}