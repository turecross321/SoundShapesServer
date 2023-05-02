using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Game.RecentActivity;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiNewsManagementEndpoints : EndpointGroup
{
    [ApiEndpoint("news/create", Method.Post)]
    public Response CreateNewsEntry(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry createdNewsEntry = database.CreateNewsEntry(body);
        return new Response(new ApiNewsResponse(createdNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("news/{id}/edit", Method.Post)]
    public Response EditNewsEntry(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;

        NewsEntry editedNewsEntry = database.EditNewsEntry(newsEntry, body);
        return new Response(new ApiNewsResponse(editedNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("news/{id}/setImage", Method.Post)]
    public Response SetNewsAssets(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, string id, Stream body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        byte[] image;

        using (MemoryStream memoryStream = new ())
        {
            body.CopyTo(memoryStream);
            image = memoryStream.ToArray();
        }
        
        if (!IsByteArrayPng(image)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        dataStore.WriteToStore(GetNewsResourceKey(id), image);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("news/{id}/remove", Method.Post)]
    public Response DeleteNewsEntry(RequestContext context, RealmDatabaseContext database, GameUser user, string id)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNewsEntryWithId(id);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        database.RemoveNewsEntry(newsEntry);
        return HttpStatusCode.OK;
    }
}