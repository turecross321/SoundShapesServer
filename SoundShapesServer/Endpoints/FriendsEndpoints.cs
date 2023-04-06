using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints;

public class FriendsEndpoints : EndpointGroup
{
    [Endpoint("/otg/~identity:{id}/~friends.all", ContentType.Json)]
    public string? GetFriends(RequestContext context, string id, RealmDatabaseContext database)                                       
    {
        GameUser? user = database.GetUserWithId(id);

        return user?.friends;
    }

    [Endpoint("/identity/person/{id}/data/psn/friends-list", ContentType.Json, Method.Post)]
    [AllowEmptyBody]    
    public Response UploadFriends(RequestContext context, RealmDatabaseContext database, string? body, string id, GameUser user)
    {
        if (body != null) database.UploadFriends(body, user);
        return new Response(HttpStatusCode.OK);
    }

    [Endpoint("/identity/person/", ContentType.Json, Method.Post)]
    public Response GetPerson(RequestContext context)
    {
        return new Response(HttpStatusCode.OK);
    }
}