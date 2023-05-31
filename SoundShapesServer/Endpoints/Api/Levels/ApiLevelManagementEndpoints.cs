using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Bunkum.ProfanityFilter;
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
    public Response CreateLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiPublishLevelRequest body, ProfanityService profanity)
    {
        if (IsUserAdmin(user) == false) return HttpStatusCode.Unauthorized;

        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.CreateLevel(user, new PublishLevelRequest(body));
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

    [ApiEndpoint("levels/id/{id}/edit", Method.Post)]
    public Response EditLevel(RequestContext context, GameDatabaseContext database, ProfanityService profanity, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }

        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("levels/id/{id}/remove", Method.Post)]
    public Response RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }

        database.RemoveLevel(level, dataStore);
        return HttpStatusCode.OK;
    }
}