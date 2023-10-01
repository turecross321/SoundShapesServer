using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints;

public class GameEndpointAttribute : HttpEndpointAttribute
{
    public const string BaseRoute = "/otg/";
    
    public GameEndpointAttribute(string route, HttpMethods method = HttpMethods.Get, ContentType contentType = ContentType.Json)
        : base(BaseRoute + route, method, contentType)
    {}

    public GameEndpointAttribute(string route, ContentType contentType, HttpMethods method = HttpMethods.Get) 
        : base(BaseRoute + route, contentType, method)
    {}
}