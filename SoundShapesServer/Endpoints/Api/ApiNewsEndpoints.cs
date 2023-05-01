using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api;

public class ApiNewsEndpoints : EndpointGroup
{
    [ApiEndpoint("news/{language}")]
    [Authentication(false)]
    public ApiNewsResponse? News(RequestContext context, RealmDatabaseContext database, string language)
    {
        NewsEntry? news = database.GetNews(language);
        return news == null ? null : new ApiNewsResponse(news);
    }
    
    // Todo: revamp this system entirely
    // todo: add it in api docs
    
    // News Creation is in ./Moderation/ApiCreateNewsEndpoint.cs
}