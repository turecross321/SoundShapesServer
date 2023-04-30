using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiManageLevelEndpoints : EndpointGroup
{
    [ApiEndpoint("level/{id}/remove", Method.Post)]
    public Response RemoveLevel(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        GameLevel? levelToRemove = database.GetLevelWithId(id);
        if (levelToRemove == null) return HttpStatusCode.NotFound;

        database.RemoveLevel(levelToRemove, dataStore);
        return HttpStatusCode.OK;
    }
}