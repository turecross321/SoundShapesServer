using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Endpoints;

namespace SoundShapesServer.Endpoints;

public class ApiEndpointAttribute : EndpointAttribute
{
    public const string BaseRoute = "/api/v1/";
    
    public string RouteWithParameters { get; set; }

    public ApiEndpointAttribute(string route, Method method = Method.Get, ContentType contentType = ContentType.Json)
        : base(BaseRoute + route, method, contentType)
    {
        RouteWithParameters = '/' + route;
    }
}