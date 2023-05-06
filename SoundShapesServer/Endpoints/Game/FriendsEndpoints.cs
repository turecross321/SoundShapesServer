using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game;

public class FriendsEndpoints : EndpointGroup
{
    // Game doesn't do anything with this, unless it's for Recent Activity?
    [GameEndpoint("~identity:{id}/~friends.all", ContentType.Json)]
    public FriendsResponse GetFriends(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        return new FriendsResponse();
    }

    [Endpoint("/identity/person/{id}/data/psn/friends-list", ContentType.Json, Method.Post)]
    [AllowEmptyBody]    
    public Response UploadFriends(RequestContext context, RealmDatabaseContext database, string? body, string id, GameUser user)
    {
        return new Response(HttpStatusCode.OK);
    }

    [Endpoint("/identity/person/", ContentType.Json, Method.Post)]
    public Response GetPerson(RequestContext context)
    {
        return new Response(HttpStatusCode.OK);
    }
}