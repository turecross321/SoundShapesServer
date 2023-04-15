using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Configuration;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Endpoints.Game;

public class EulaEndpoint : EndpointGroup
{
    // Gets called by AuthenticationEndpoints.cs
    public static string NormalEula(GameServerConfig config)
    {
        return config.EulaText;
    }
}