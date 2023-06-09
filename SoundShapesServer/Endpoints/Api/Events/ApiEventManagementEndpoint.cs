using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventManagementEndpoint : EndpointGroup
{
    [ApiEndpoint("events/id/{id}/remove", Method.Post)]
    public Response RemoveEvent(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserModeratorOrMore(user) == false) return HttpStatusCode.Forbidden;

        GameEvent? eventObject = database.GetEventWithId(id);
        if (eventObject == null) return HttpStatusCode.NotFound;
        
        database.RemoveEvent(eventObject);
        return HttpStatusCode.OK;
    }
}