using System.Net;
using System.Text;
using Bunkum.CustomHttpListener.Request;
using Bunkum.HttpServer.Database;
using Bunkum.HttpServer.Endpoints.Middlewares;

namespace SoundShapesServer.Middlewares;

public class FileSizeMiddleware : IMiddleware
{
    private const long FileSizeLimit = 4000000; // 4 mb
    
    public void HandleRequest(ListenerContext context, Lazy<IDatabaseContext> database, Action next)
    {
        if (context.InputStream.Length >= FileSizeLimit)
        {
            Console.WriteLine("User attempted to upload a file that exceeded 4 megabytes. Denying request.");
            
            context.ResponseCode = HttpStatusCode.BadRequest;
            context.ResponseStream.Write("File size exceeded 4 megabytes!"u8);
            return;
        }

        next();
    }
}