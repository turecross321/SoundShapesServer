using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Endpoints.Middlewares;
using SoundShapesServer.Endpoints;

namespace SoundShapesServer.Middlewares;

public class LimitContentSizeMiddleware : IMiddleware
{
    private const long GameFileSizeLimit = 2000000; // 1 mb
    private const long ApiFileSizeLimit = 16000000; // 16 mb
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        if (context.Uri.AbsolutePath.StartsWith(ApiEndpointAttribute.BaseRoute))
        {
            Console.WriteLine("ContentLength exceeded the API file size limit. Denying request...");
            if (context.ContentLength > ApiFileSizeLimit) return;
        }
        else
        {
            Console.WriteLine("ContentLength exceeded the Game file size limit. Denying request...");
            if (context.ContentLength > GameFileSizeLimit) return;
        }
        
        next();
    }
}