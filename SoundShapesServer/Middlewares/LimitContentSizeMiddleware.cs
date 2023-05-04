using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Endpoints.Middlewares;
using SoundShapesServer.Endpoints;

namespace SoundShapesServer.Middlewares;

public class LimitContentSizeMiddleware : IMiddleware
{
    private const long FileSizeLimit = 8000000; // 8 mb
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        if (context.ContentLength > FileSizeLimit)
        {
            Console.WriteLine("ContentLength exceeded the file size limit. Denying request...");
            return;
        }

        next();
    }
}