using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.RecentActivity;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.News;

public class ApiNewsManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("news/create", Method.Post)]
    public Response CreateNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry createdNewsEntry = database.CreateNewsEntry(body, user);
        return new Response(new ApiNewsResponse(createdNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("news/id/{id}/edit", Method.Post)]
    public Response EditNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        NewsEntry editedNewsEntry = database.EditNewsEntry(newsEntry, body);
        return new Response(new ApiNewsResponse(editedNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("news/id/{id}/setThumbnail", Method.Post)]
    public Response SetNewsAssets(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id, byte[] body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        return database.UploadNewsResource(dataStore, newsEntry, body);
    }

    [ApiEndpoint("news/id/{id}/remove", Method.Post)]
    public Response DeleteNewsEntry(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        database.RemoveNewsEntry(dataStore, newsEntry);
        return HttpStatusCode.OK;
    }
}