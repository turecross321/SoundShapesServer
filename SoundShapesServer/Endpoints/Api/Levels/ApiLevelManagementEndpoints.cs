using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Storage;
using Bunkum.ProfanityFilter;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/create", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates level.")]
    public ApiResponse<ApiLevelFullResponse> CreateLevel(RequestContext context, GameDatabaseContext database,
        GameUser user, ApiCreateLevelRequest body, ProfanityService profanity)
    {
        DateTimeOffset now = DateTimeOffset.UtcNow;
        GameLevel level = new()
        {
            Id = IdHelper.GenerateLevelId(),
            Name = profanity.CensorSentence(body.Name),
            Language = body.Language,
            CreationDate = body.CreationDate ?? now,
            ModificationDate = body.CreationDate ?? now,
            Author = user,
            Visibility = body.Visibility,
            UploadPlatform = PlatformType.Unknown
        };

        database.AddLevel(level, true);
        return ApiLevelFullResponse.FromOld(level);
    }

    [ApiEndpoint("levels/id/{id}/setLevel", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the level file of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.CorruptLevelWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiOkResponse UploadLevelFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
    {
        return UploadLevelResource(database, dataStore, body, id, FileType.Level);
    }

    [ApiEndpoint("levels/id/{id}/setSound", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the sound file of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiOkResponse UploadSoundFile
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
    {
        return UploadLevelResource(database, dataStore, body, id, FileType.Sound);
    }

    [ApiEndpoint("levels/id/{id}/setThumbnail", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets the thumbnail of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiOkResponse UploadThumbnail
    (RequestContext context, GameDatabaseContext database, IDataStore dataStore, byte[] body,
        string id)
    {
        return UploadLevelResource(database, dataStore, body, id, FileType.Image);
    }

    private ApiOkResponse UploadLevelResource(GameDatabaseContext database, IDataStore dataStore,
        byte[] body, string id, FileType fileType)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        return database.UploadLevelResource(dataStore, level, body, fileType);
    }

    [ApiEndpoint("levels/id/{id}/edit", HttpMethods.Post)]
    [DocSummary("Edits level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoEditPermissionWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiResponse<ApiLevelFullResponse> EditLevel(RequestContext context, GameDatabaseContext database,
        ProfanityService profanity, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        if (level.Author.Id != user.Id)
            if (user.PermissionsType < PermissionsType.Moderator)
                return ApiUnauthorizedError.NoEditPermission;


        level = database.EditLevel(level, profanity.CensorSentence(body.Name), body.Language, body.Visibility);
        return ApiLevelFullResponse.FromOld(level);
    }

    [ApiEndpoint("levels/id/{id}", HttpMethods.Delete)]
    [DocSummary("Deletes level with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.NoDeletionPermissionWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiOkResponse RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore,
        GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        if (level.Author.Id != user.Id)
            if (user.PermissionsType < PermissionsType.Moderator)
                return ApiUnauthorizedError.NoDeletionPermission;

        database.RemoveLevel(level, dataStore);
        return new ApiOkResponse();
    }
}