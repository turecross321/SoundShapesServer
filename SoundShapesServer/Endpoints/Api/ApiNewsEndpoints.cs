using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.NewsHelper;
using static SoundShapesServer.Helpers.ResourceHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news/{language}")]
    [Authentication(false)]
    public ApiNewsResponse News(RequestContext context, RealmDatabaseContext database, string language)
    {
        NewsEntry? news = database.GetNews(language);
        return NewsEntryToNewsApiResponse(news);
    }
    
    [ApiEndpoint("news/create", Method.Post)]
    public Response CreateNewsEntry(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, Stream body, GameUser user)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;
        
        MultipartFormDataParser request = MultipartFormDataParser.Parse(body);

        string language;
        string title;
        string summary;
        string fullText;
        string url;

        byte[] image;
        
        try
        {
            language = request.Parameters.First(p => p.Name == "Language").Data;
            title = request.Parameters.First(p => p.Name == "Title").Data;
            summary = request.Parameters.First(p => p.Name == "Summary").Data;
            fullText = request.Parameters.First(p => p.Name == "FullText").Data;
            url = request.Parameters.First(p => p.Name == "Url").Data;

            image = FilePartToBytes(request.Files.First(p => p.Name == "Image"));
        }
        catch (InvalidOperationException e)
        {
            return HttpStatusCode.BadRequest;
        }
        
        if (!IsByteArrayPng(image)) return new Response("Image is not a PNG.", ContentType.Plaintext, HttpStatusCode.BadRequest);

        dataStore.WriteToStore(GetNewsResourceKey(language), image);
        database.CreateNewsEntry(language, title, summary, fullText, url);

        return HttpStatusCode.OK;
    }
}