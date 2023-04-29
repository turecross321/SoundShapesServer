using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.NewsHelper;

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