using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Events;

public class ApiEventManagementEndpoint : EndpointGroup
{
    [ApiEndpoint("events/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Moderator)]
    [DocSummary("Deletes event with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.EventNotFoundWhen)]
    public ApiOkResponse RemoveEvent(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameEvent? eventObject = database.GetEventWithId(id);
        if (eventObject == null) 
            return ApiNotFoundError.EventNotFound;
        
        database.RemoveEvent(eventObject);
        return new ApiOkResponse();
    }
}