using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiNewsCreationEndpoint : EndpointGroup
{
    [ApiEndpoint("news/create", Method.Post)]
    public Response CreateNewsEntry(RequestContext context, RealmDatabaseContext database, ApiNewsEntryRequest body, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;
        
        database.CreateNewsEntry(body);

        return HttpStatusCode.OK;
    }

    [ApiEndpoint("news/{language}/setImage", Method.Post)]
    public Response SetNewsImage(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, byte[] body, string language)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        NewsEntry? newsEntry = database.GetNews(language);
        if (newsEntry == null) return HttpStatusCode.NotFound;
        
        if (!IsByteArrayPng(body)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        dataStore.WriteToStore(GetNewsResourceKey(newsEntry.Language), body);
        return HttpStatusCode.OK;
    }
}