using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.PermissionHelper;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/create", Method.Post)]
    public Response CreateLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiPublishLevelRequest body)
    {
        if (IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameLevel publishedLevel = database.CreateLevel(new PublishLevelRequest(body), user);
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("levels/id/{id}/setLevel", Method.Post)]
    public Response UploadLevelFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, user, body, id, FileType.Level);
    
    [ApiEndpoint("levels/id/{id}/setSound", Method.Post)]
    public Response UploadSoundFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, user, body, id, FileType.Sound);
    
    [ApiEndpoint("levels/id/{id}/setThumbnail", Method.Post)]
    public Response UploadThumbnail
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, user, body, id, FileType.Image);

    private Response UploadLevelResource(GameDatabaseContext database, IDataStore dataStore, GameUser user, 
        byte[] body, string id, FileType fileType)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id && !IsUserAdmin(user))
            return HttpStatusCode.Unauthorized;

        return database.UploadLevelResource(dataStore, level, body, fileType);
    }

    // Remove Levels is in ../Levels/ApiLevelEndpoints
}