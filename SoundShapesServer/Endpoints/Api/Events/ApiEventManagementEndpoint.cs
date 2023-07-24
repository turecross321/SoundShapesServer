using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventManagementEndpoint : EndpointGroup
{
    [ApiEndpoint("events/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Deletes event with specified ID.")]
    public Response RemoveEvent(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameEvent? eventObject = database.GetEventWithId(id);
        if (eventObject == null) return HttpStatusCode.NotFound;
        
        database.RemoveEvent(eventObject);
        return HttpStatusCode.OK;
    }
}