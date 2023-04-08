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
    private const string savesPath = "saves";
    
    [Endpoint("/otg/~identity:{userId}/~content:progress.put", Method.Post)]
    public Response UploadSave(RequestContext context, GameUser user, string userId, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);
        
        string saveString = parser.GetParameterValue("file");
        byte[] newSave = Encoding.UTF8.GetBytes(saveString);

        // I shit you not, this is how the game tells the server it wants to delete the save:
        if (CloudSyncHelper.IsSaveEmpty(newSave)) return DeleteSave(context, user); 

        byte[] combinedSave = newSave;

        string key = $"{savesPath}/{user.id}";
        
        // If there's an old one, combine them
        if (context.DataStore.TryGetDataFromStore(key, out byte[]? oldSave))
        {
            combinedSave = CloudSyncHelper.CombineSaves(oldSave, newSave);
        }

        context.DataStore.WriteToStore(key, combinedSave);

        return HttpStatusCode.OK;
    }

    private Response DeleteSave(RequestContext context, GameUser user)
    {
        string key = $"{savesPath}/{user.id}";

        if (!context.DataStore.RemoveFromStore(key)) return HttpStatusCode.InternalServerError;
        else return HttpStatusCode.OK;
    }

    [Endpoint("/otg/~identity:{userId}/~content:progress/data.get")]
    public Response DownloadSave(RequestContext context, GameUser user, string userId)
    {
        string key = $"{savesPath}/{user.id}";

        if (context.DataStore.TryGetDataFromStore(key, out byte[]? byteArray) == false)
            return HttpStatusCode.NotFound;

        return new Response(byteArray, ContentType.BinaryData);
    }
}