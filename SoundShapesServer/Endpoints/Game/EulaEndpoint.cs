using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Configuration;

namespace SoundShapesServer.Endpoints.Game;

public class EulaEndpoint : EndpointGroup
{
    [GameEndpoint("{platform}/{publisher}/{language}/~eula.get", ContentType.Json)]
    public string Eula(RequestContext context, GameServerConfig config, string platform, string publisher, string language)
    {
        return config.EulaText;
    }
}