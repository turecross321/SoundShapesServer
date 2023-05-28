using System.Net;
using System.Text;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using Bunkum.RealmDatabase;
using HttpMultipartParser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class CloudSyncEndpoints : EndpointGroup
{
    [GameEndpoint("~identity:{userId}/~content:progress.put", Method.Post)]
    public Response UploadSave(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, Stream body)
    {
        string? key = user.SaveFilePath;
        
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);
        
        string saveString = parser.GetParameterValue("file");
        byte[] newSave = Encoding.UTF8.GetBytes(saveString);

        // I shit you not, this is how the game tells the server it wants to delete the save:
        if (CloudSyncHelper.IsSaveEmpty(newSave)) return DeleteSave(database, dataStore, user); 

        byte[] combinedSave = newSave;

        if (key != null)
        {
            // If there's an old save, combine them
            if (dataStore.ExistsInStore(key))
            {
                byte[] oldSave = dataStore.GetDataFromStore(key);
                combinedSave = CloudSyncHelper.CombineSaves(oldSave, newSave);
            }
        }

        key ??= ResourceHelper.GetSaveResourceKey(user.Id);
        
        database.SetUserSaveFilePath(user, key);
        dataStore.WriteToStore(key, combinedSave);

        return HttpStatusCode.OK;
    }

    private Response DeleteSave(GameDatabaseContext database, IDataStore dataStore, GameUser user)
    {
        string key = ResourceHelper.GetSaveResourceKey(user.Id);
        dataStore.RemoveFromStore(key);
        database.SetUserSaveFilePath(user, null);
        return HttpStatusCode.OK;
    }

    [GameEndpoint("~identity:{userId}/~content:progress/data.get")]
    public Response GetSave(RequestContext context, IDataStore dataStore, GameUser user, string userId)
    {
        string? key = user.SaveFilePath;
        if (key == null) return HttpStatusCode.NotFound;
        if (!dataStore.ExistsInStore(key)) return HttpStatusCode.Gone;

        byte[] bytes = dataStore.GetDataFromStore(key);

        // This next part sets the onlineID parameter to the user's username.
        // This makes so cloud saves don't break when a user changes their username.
        
        // Convert save byte array to string
        string jsonString = Encoding.UTF8.GetString(bytes);
        // Parse the string to a Newtonsoft JObject
        JObject? json = JsonConvert.DeserializeObject<JObject>(jsonString);
        if (json == null) return HttpStatusCode.InternalServerError;

        json["onlineID"] = user.Username;
        
        // Serialize the JObject to a JSON string
        string responseString = JsonConvert.SerializeObject(json);

        // Convert the JSON string to a byte array
        byte[] responseData = Encoding.UTF8.GetBytes(responseString);
        
        return new Response(responseData, ContentType.BinaryData);
    }
}