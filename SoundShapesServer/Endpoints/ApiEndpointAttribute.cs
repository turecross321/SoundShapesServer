using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints;

public class ApiEndpointAttribute: HttpEndpointAttribute
{
    public const string RoutePrefix = "/api/v1";
    
    public ApiEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, 
        string contentType = Bunkum.Listener.Protocol.ContentType.Json) : 
        base($"{RoutePrefix}/{route}", method, contentType)
    {
    }

    public ApiEndpointAttribute(string route, string contentType) : base($"{RoutePrefix}/{route}", contentType)
    {
    }

    public ApiEndpointAttribute(string route, string contentType, HttpMethods method) : base($"{RoutePrefix}/{route}", contentType, method)
    {
    }
}