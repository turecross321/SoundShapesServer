using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Endpoints;

namespace SoundShapesServer.Endpoints;

public class ApiEndpointAttribute : EndpointAttribute
{
    public const string BaseRoute = "/api/v1/";

    public ApiEndpointAttribute(string route, Method method = Method.Get, ContentType contentType = ContentType.Json) 
        : base(BaseRoute + route, method, contentType)
    {}

    public ApiEndpointAttribute(string route, ContentType contentType, Method method = Method.Get) 
        : base(BaseRoute + route, contentType, method)
    {}
}