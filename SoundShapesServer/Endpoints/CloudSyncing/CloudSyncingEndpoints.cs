using System.Net;
using System.Text;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.CloudSyncing;

public class CloudSyncingEndpoints : EndpointGroup
{

    [Endpoint("/otg/~identity:{userId}/~content:progress.put", Method.Post)]
    public Response UploadSave(RequestContext context, GameUser user, string userId, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);
        
        string saveString = parser.GetParameterValue("file");
        byte[] newSave = Encoding.UTF8.GetBytes(saveString);

        byte[] combinedSave = newSave;
        
        if (context.DataStore.TryGetDataFromStore(user.id + "_save", out byte[]? oldSave))
        {
            combinedSave = CloudSyncHelper.CombineSaves(oldSave, newSave);
        }

        context.DataStore.WriteToStore(user.id + "_save", combinedSave);

        return HttpStatusCode.OK;
    }

    [Endpoint("/otg/~identity:{userId}/~content:progress/data.get")]
    public Response DownloadSave(RequestContext context, GameUser user, string userId)
    {
        if (context.DataStore.TryGetDataFromStore(user.id + "_save", out byte[]? byteArray) == false)
            return HttpStatusCode.NotFound;

        return new Response(byteArray, ContentType.BinaryData);
    }
}