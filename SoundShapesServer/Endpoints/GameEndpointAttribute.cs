using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints;

public class GameEndpointAttribute: HttpEndpointAttribute
{
    public const string RoutePrefix = "/otg";
    
    public GameEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, 
        string contentType = Bunkum.Listener.Protocol.ContentType.Json) : 
        base($"{RoutePrefix}/{route}", method, contentType)
    {
    }

    public GameEndpointAttribute(string route, string contentType) : base($"{RoutePrefix}/{route}", contentType)
    {
    }

    public GameEndpointAttribute(string route, string contentType, HttpMethods method) : base($"{RoutePrefix}/{route}", contentType, method)
    {
    }
}