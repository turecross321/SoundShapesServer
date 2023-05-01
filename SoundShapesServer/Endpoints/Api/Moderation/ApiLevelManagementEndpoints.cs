using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using Realms;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/create", Method.Post)]
    public Response PublishLevel(RequestContext context, RealmDatabaseContext database, GameUser user, ApiPublishLevelRequest body)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        string levelId = LevelHelper.GenerateLevelId(database);
        
        GameLevel publishedLevel = database.PublishLevel(new PublishLevelRequest(body, levelId), user);
        return new Response(new ApiLevelFullResponse(publishedLevel, user), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("level/{id}/edit")]
    public Response EditLevel(RequestContext context, RealmDatabaseContext database, GameUser user,
        ApiPublishLevelRequest body, string id)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body, id), level, user);
        return new Response(new ApiLevelFullResponse(publishedLevel, user), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("level/{id}/setLevel")]
    public Response UploadLevelFile
    (RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body,
        string id)
        => UploadLevelResources(database, dataStore, user, body, id, FileType.Level);
    
    [ApiEndpoint("level/{id}/setSound")]
    public Response UploadSoundFile
    (RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body,
        string id)
        => UploadLevelResources(database, dataStore, user, body, id, FileType.Sound);
    
    [ApiEndpoint("level/{id}/setThumbnail")]
    public Response UploadThumbnail
    (RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body,
        string id)
        => UploadLevelResources(database, dataStore, user, body, id, FileType.Image);

    private Response UploadLevelResources(RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body, string id, FileType fileType)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;
        
        byte[] file;

        using (MemoryStream memoryStream = new ())
        {
            body.CopyTo(memoryStream);
            file = memoryStream.ToArray();
        }
        
        if (fileType == FileType.Image && !IsByteArrayPng(file)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        string key = GetLevelResourceKey(id, fileType);
        dataStore.WriteToStore(key, file);
        if (fileType == FileType.Level) database.SetLevelFileSize(level, file.Length);

        return HttpStatusCode.Created;
    }
    
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