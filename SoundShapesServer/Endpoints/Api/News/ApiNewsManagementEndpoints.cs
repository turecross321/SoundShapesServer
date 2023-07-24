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
using SoundShapesServer.Responses.Api.News;
using SoundShapesServer.Types;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("news/create", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Creates news entry.")]
    public Response CreateNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body)
    {
        NewsEntry createdNewsEntry = database.CreateNewsEntry(body, user);
        return new Response(new ApiNewsResponse(createdNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("news/id/{id}/edit", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Edits news entry with specified ID.")]
    public Response EditNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        NewsEntry editedNewsEntry = database.EditNewsEntry(newsEntry, body, user);
        return new Response(new ApiNewsResponse(editedNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("news/id/{id}/setThumbnail", Method.Post)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Sets thumbnail of news entry with specified ID.")]
    public Response SetNewsAssets(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        return database.UploadNewsResource(dataStore, newsEntry, body);
    }

    [ApiEndpoint("news/id/{id}", Method.Delete)]
    [MinimumPermissions(PermissionsType.Administrator)]
    [DocSummary("Deletes news entry with specified ID.")]
    public Response DeleteNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        database.RemoveNewsEntry(dataStore, newsEntry);
        return HttpStatusCode.OK;
    }
}