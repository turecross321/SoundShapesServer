using Bunkum.Protocols.Http;

namespace SoundShapesServer.Endpoints;

public class ApiEndpointAttribute(
    string route,
    HttpMethods method = HttpMethods.Get,
    string contentType = Bunkum.Listener.Protocol.ContentType.Json)
    : HttpEndpointAttribute($"{RoutePrefix}/{route}", method, contentType)
{
    public const string RoutePrefix = "/api/v1";

    public string Route = route;
}