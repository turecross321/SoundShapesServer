using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Configuration;

namespace SoundShapesServer.Endpoints.Game;

public class EulaEndpoint : EndpointGroup
{
    // Gets called by AuthenticationEndpoints.cs
    public static string NormalEula(GameServerConfig config)
    {
        return config.EulaText;
    }
}