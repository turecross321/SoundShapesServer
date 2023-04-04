using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.NewsHelper;

namespace SoundShapesServer.Endpoints.RecentActivity;

public class NewsEndpoints : EndpointGroup
{
    [Endpoint("/otg/global/news/~metadata:*.get", ContentType.Plaintext)]
    public JArray GlobalNews(RequestContext context, RealmDatabaseContext database)
    {
        NewsEntry? news = database.GetGlobalNews();

        string text = news.text;
        string title = news.title;
        string fullText = news.fullText;
        string url = news.url;
        
        JArray json = SerializeNews(text, title, fullText, url);
        
        return json;
    }

    [Endpoint("/otg/global/news/{language}/~metadata:*.get", ContentType.Plaintext)]
    public JArray TranslatedNews(RequestContext context, GameSession session, RealmDatabaseContext database, string language)
    {
        NewsEntry? news = database.GetTranslatedNews(language);
        
        string text = news.text;
        string title = news.title;
        string fullText = news.fullText;
        string url = news.url;

        JArray json = SerializeNews(text, title, fullText, url);
        
        return json;
    }
}