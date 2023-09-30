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
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
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
    public ApiResponse<ApiLevelFullResponse> CreateLevel(RequestContext context, GameDatabaseContext database, GameUser user, ApiPublishLevelRequest body, ProfanityService profanity)
    {
        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.CreateLevel(user, new PublishLevelRequest(body), PlatformType.Api);
        return new ApiLevelFullResponse(publishedLevel);
    }

    [ApiEndpoint("levels/id/{id}/setLevel", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the level file of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.CorruptLevelWhen)]
    public ApiOkResponse UploadLevelFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Level);
    
    [ApiEndpoint("levels/id/{id}/setSound", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the sound file of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiOkResponse UploadSoundFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Sound);
    
    [ApiEndpoint("levels/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the thumbnail of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    public ApiOkResponse UploadThumbnail
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
        => UploadLevelResource(database, dataStore, body, id, FileType.Image);

    private ApiOkResponse UploadLevelResource(GameDatabaseContext database, IDataStore dataStore, 
        byte[] body, string id, FileType fileType)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        return database.UploadLevelResource(dataStore, level, body, fileType);
    }

    [ApiEndpoint("levels/id/{id}/edit", Method.Post)]
    [DocSummary("Edits level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoEditPermissionWhen)]
    public ApiResponse<ApiLevelFullResponse> EditLevel(RequestContext context, GameDatabaseContext database, ProfanityService profanity, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) 
            return ApiNotFoundError.LevelNotFound;

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return ApiUnauthorizedError.NoEditPermission;
        }

        body.Name = profanity.CensorSentence(body.Name); // Censor any potential profanity
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new ApiLevelFullResponse(publishedLevel);
    }
    
    [ApiEndpoint("levels/id/{id}", Method.Delete)]
    [DocSummary("Deletes level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoDeletionPermissionWhen)]
    public ApiOkResponse RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        if (level.Author.Id != user.Id)
        {
            if (IsUserModeratorOrMore(user) == false)
                return ApiUnauthorizedError.NoDeletionPermission;
        }

        database.RemoveLevel(level, dataStore);
        return new ApiOkResponse();
    }
}