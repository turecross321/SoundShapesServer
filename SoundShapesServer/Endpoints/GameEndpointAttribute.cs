using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer.Endpoints;

namespace SoundShapesServer.Endpoints;

public class GameEndpointAttribute : EndpointAttribute
{
    private const string BaseRoute = "/g/";
    
    public GameEndpointAttribute(string route, Method method = Method.Get, ContentType contentType = ContentType.Plaintext)
        : base(BaseRoute + route, method, contentType)
    {}

    public GameEndpointAttribute(string route, ContentType contentType, Method method = Method.Get) 
        : base(BaseRoute + route, contentType, method)
    {}
}