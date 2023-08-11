using System.Net;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Endpoints.Middlewares;

namespace SoundShapesServer.Middlewares;

public class FileSizeMiddleware : IMiddleware
{
    private const long FileSizeLimit = Globals.FourMegabytes;
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        if (context.InputStream.Length >= FileSizeLimit)
        {
            Console.WriteLine("User attempted to upload a file that exceeded 4 megabytes. Denying request.");
            context.ResponseCode = HttpStatusCode.RequestEntityTooLarge;
            return;
        }

        next();
    }
}