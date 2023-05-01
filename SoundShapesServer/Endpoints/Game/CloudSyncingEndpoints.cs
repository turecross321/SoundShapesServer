using System.Net;
using System.Text;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Game.CloudSyncing;

public class CloudSyncingEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{userId}/~content:progress.put", Method.Post)]
    public Response UploadSave(RequestContext context, IDataStore dataStore, GameUser user, string userId, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);
        
        string saveString = parser.GetParameterValue("file");
        byte[] newSave = Encoding.UTF8.GetBytes(saveString);

        // I shit you not, this is how the game tells the server it wants to delete the save:
        if (CloudSyncHelper.IsSaveEmpty(newSave)) return DeleteSave(dataStore, user); 

        byte[] combinedSave = newSave;

        string key = ResourceHelper.GetSaveResourceKey(user.Id);
        
        // If there's an old one, combine them
        if (dataStore.TryGetDataFromStore(key, out byte[]? oldSave))
        {
            if (oldSave != null)
                combinedSave = CloudSyncHelper.CombineSaves(oldSave, newSave);
        }

        dataStore.WriteToStore(key, combinedSave);

        return HttpStatusCode.OK;
    }

    private Response DeleteSave(IDataStore dataStore, GameUser user)
    {
        string key = ResourceHelper.GetSaveResourceKey(user.Id);

        return dataStore.RemoveFromStore(key) ? HttpStatusCode.OK : HttpStatusCode.InternalServerError;
    }

    [GameEndpoint("~identity:{userId}/~content:progress/data.get")]
    public Response DownloadSave(RequestContext context, IDataStore dataStore, GameUser user, string userId)
    {
        string key = ResourceHelper.GetSaveResourceKey(user.Id);

        if (dataStore.TryGetDataFromStore(key, out byte[]? bytes) == false)
            return HttpStatusCode.NotFound;

        return bytes != null ? new Response(bytes, ContentType.BinaryData) : HttpStatusCode.NotFound;
    }
}