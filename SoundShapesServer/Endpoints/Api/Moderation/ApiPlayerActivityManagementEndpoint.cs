using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiPlayerActivityManagementEndpoint : EndpointGroup
{
    [ApiEndpoint("activities/{id}/remove", Method.Post)]
    public Response RemoveActivity(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        GameEvent? eventObject = database.GetEventWithId(id);
        if (eventObject == null) return HttpStatusCode.NotFound;
        
        database.RemoveEvent(eventObject);
        return HttpStatusCode.OK;
    }
}