using System.Net;
using System.Text;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Game;

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
    public Response GetSave(RequestContext context, IDataStore dataStore, GameUser user, string userId)
    {
        string key = ResourceHelper.GetSaveResourceKey(user.Id);

        dataStore.TryGetDataFromStore(key, out byte[]? bytes);
        if (bytes == null) return HttpStatusCode.NotFound;
        
        // This next part here sets the onlineID parameter to the user's username.
        // This makes so cloud saves don't break when a user changes their username.
        
        // convert save byte array to string
        string jsonString = Encoding.UTF8.GetString(bytes);
        // parse the string to a Newtonsoft JObject
        JObject? json = JsonConvert.DeserializeObject<JObject>(jsonString);
        if (json == null) return HttpStatusCode.InternalServerError;

        json["onlineID"] = user.Username;
        
        // serialize the JObject to a JSON string
        string responseString = JsonConvert.SerializeObject(json);

        // convert the JSON string to a byte array
        byte[] responseData = Encoding.UTF8.GetBytes(responseString);
        
        return new Response(responseData, ContentType.BinaryData);
    }
}