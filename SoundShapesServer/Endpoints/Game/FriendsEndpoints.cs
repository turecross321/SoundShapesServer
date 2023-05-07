using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game;

public class FriendsEndpoints : EndpointGroup
{
    // Todo: Figure out what the response here should actually be. I know it should be a json, but that's all I know.
    [GameEndpoint("~identity:{id}/~friends.all", ContentType.Json)]
    public Response GetFriends(RequestContext context)
    {
        return HttpStatusCode.OK;
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