using Bunkum.Core.Endpoints;
using SoundShapesServer.Configuration;

namespace SoundShapesServer.Endpoints.Game;

// ReSharper disable once ClassNeverInstantiated.Global
public class EulaEndpoint : EndpointGroup
{
    // Gets called by AuthenticationEndpoints.cs
    public static string NormalEula(GameServerConfig config)
    {
        return config.EulaText + "\n \n" + Globals.AGPLLicense;
    }
}