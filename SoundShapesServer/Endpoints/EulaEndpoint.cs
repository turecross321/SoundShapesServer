using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Configuration;

namespace SoundShapesServer.Endpoints;

public class EulaEndpoint : EndpointGroup
{
    [Endpoint("/otg/{platform}/{publisher}/{language}/~eula.get", ContentType.Json)]
    public string Eula(RequestContext context, GameServerConfig config, string platform, string publisher, string language)
    {
        return config.EulaText;
    }
}