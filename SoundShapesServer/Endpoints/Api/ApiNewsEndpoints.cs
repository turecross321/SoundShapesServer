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
    
    // News Creation is in ./Moderation/ApiCreateNewsEndpoint.cs
}