using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Bunkum.ProfanityFilter;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Errors;
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
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates level.")]
    public Response CreateLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiPublishLevelRequest body, ProfanityService profanity)
    {
        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.CreateLevel(user, new PublishLevelRequest(body));
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("levels/id/{id}/setLevel", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the level file of level.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(BadRequestError), BadRequestError.CorruptLevelWhen)]
    public Response UploadLevelFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Level);
    
    [ApiEndpoint("levels/id/{id}/setSound", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the sound file of level.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    public Response UploadSoundFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Sound);
    
    [ApiEndpoint("levels/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the thumbnail of level.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(BadRequestError), BadRequestError.FileIsNotPngWhen)]
    public Response UploadThumbnail
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Image);

    private Response UploadLevelResource(GameDatabaseContext database, IDataStore dataStore, 
        byte[] body, string id, FileType fileType)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return new Response(NotFoundError.LevelNotFoundWhen, ContentType.Plaintext, HttpStatusCode.NotFound);

        return database.UploadLevelResource(dataStore, level, body, fileType);
    }

    [ApiEndpoint("levels/id/{id}/edit", Method.Post)]
    [DocSummary("Edits level with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(UnauthorizedError), UnauthorizedError.NoEditPermissionWhen)]
    public Response EditLevel(RequestContext context, GameDatabaseContext database, ProfanityService profanity, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return new Response(NotFoundError.LevelNotFoundWhen, ContentType.Plaintext, HttpStatusCode.NotFound);

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return new Response(UnauthorizedError.NoEditPermissionWhen, ContentType.Plaintext, HttpStatusCode.Unauthorized);
        }

        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("levels/id/{id}", Method.Delete)]
    [DocSummary("Deletes level with specified ID.")]
    [DocError(typeof(NotFoundError), NotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(UnauthorizedError), UnauthorizedError.NoDeletionPermissionWhen)]
    public Response RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return new Response(NotFoundError.LevelNotFoundWhen, ContentType.Plaintext, HttpStatusCode.NotFound);

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return new Response(UnauthorizedError.NoDeletionPermissionWhen, ContentType.Plaintext, HttpStatusCode.Unauthorized);
        }

        database.RemoveLevel(level, dataStore);
        return HttpStatusCode.NoContent;
    }
}