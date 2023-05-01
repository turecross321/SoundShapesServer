using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
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

public class ApiNewsManagementEndpoints
{
    [ApiEndpoint("news/create", Method.Post)]
    public Response CreateNewsEntry(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest request)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNews(request.Language);
        if (newsEntry != null) return HttpStatusCode.Conflict;
        
        NewsEntry createdNewsEntry = database.CreateNewsEntry(request);
        return new Response(new ApiNewsResponse(createdNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }

    [ApiEndpoint("news/{language}/edit", Method.Post)]
    public Response EditNewsEntry(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, 
        GameUser user, ApiCreateNewsEntryRequest body, string language)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntryThatAlreadyHasLanguage = database.GetNews(body.Language);
        if (newsEntryThatAlreadyHasLanguage != null) return HttpStatusCode.Conflict;

        NewsEntry? newsEntry = database.GetNews(language);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        NewsEntry createdNewsEntry = database.EditNewsEntry(newsEntry, body);
        return new Response(new NewsResponse(createdNewsEntry), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("news/{language}/setImage", Method.Post)]
    public Response SetNewsAssets(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, string language, Stream body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        byte[] image;

        using (MemoryStream memoryStream = new ())
        {
            body.CopyTo(memoryStream);
            image = memoryStream.ToArray();
        }
        
        if (!IsByteArrayPng(image)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        dataStore.WriteToStore(GetNewsResourceKey(language), image);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("news/{language}/remove", Method.Post)]
    public Response DeleteNewsEntry(RequestContext context, RealmDatabaseContext database, GameUser user, string language)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNews(language);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        database.RemoveNewsEntry(newsEntry);
        return HttpStatusCode.OK;
    }
}