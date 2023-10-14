using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints;

public class ApiEndpointAttribute : HttpEndpointAttribute
{
    public const string BaseRoute = "/api/v1/";
    
    public string RouteWithParameters { get; set; }

    public ApiEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, string contentType = Bunkum.Listener.Protocol.ContentType.Json)
        : base(BaseRoute + route, method, contentType)
    {
        RouteWithParameters = '/' + route;
    }
}