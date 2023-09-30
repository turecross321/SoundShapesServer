using System.Net;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("news/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates news entry.")]
    public ApiResponse<ApiNewsResponse> CreateNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body)
    {
        NewsEntry createdNewsEntry = database.CreateNewsEntry(body, user);
        return new ApiNewsResponse(createdNewsEntry);
    }

    [ApiEndpoint("news/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits news entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NewsEntryNotFoundWhen)]
    public ApiResponse<ApiNewsResponse> EditNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) 
            return ApiNotFoundError.NewsEntryNotFound;

        NewsEntry editedNewsEntry = database.EditNewsEntry(newsEntry, body, user);
        return new ApiNewsResponse(editedNewsEntry);
    }
    
    [ApiEndpoint("news/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of news entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NewsEntryNotFoundWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.FileIsNotPngWhen)]
    public ApiOkResponse SetNewsAssets(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) 
            return ApiNotFoundError.NewsEntryNotFound;

        return database.UploadNewsResource(dataStore, newsEntry, body);
    }

    [ApiEndpoint("news/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes news entry with specified ID.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.NewsEntryNotFoundWhen)]
    public ApiOkResponse DeleteNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) 
            return ApiNotFoundError.NewsEntryNotFound;
        
        database.RemoveNewsEntry(dataStore, newsEntry);
        return new ApiOkResponse();
    }
}