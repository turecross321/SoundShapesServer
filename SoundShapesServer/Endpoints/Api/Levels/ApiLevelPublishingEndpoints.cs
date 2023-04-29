using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints.Game.Levels;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelPublishingEndpoints : EndpointGroup
{
    [ApiEndpoint("level/publish", Method.Post)]
    public Response PublishLevels(RequestContext context, RealmDatabaseContext database, IDataStore dataStore, GameUser user, Stream body)
    {
        if (PermissionHelper.IsUserAdmin(user) == false) return HttpStatusCode.Forbidden;

        MultipartFormDataParser parser = MultipartFormDataParser.Parse(body);

        string levelId = LevelHelper.GenerateLevelId(database);
        bool uploadedResources = ApiLevelResourceEndpoints.UploadLevelResources(dataStore, parser, levelId);
        if (uploadedResources == false) return HttpStatusCode.BadRequest;

        LevelPublishRequest levelRequest = new ()
        {
            Title = parser.GetParameterValue("Title"),
            Language = 0,
            Id = levelId,
            FileSize = parser.Files.First(f => f.Name == "Level").Data.Length
        };
        
        database.PublishLevel(levelRequest, user);

        return new Response(HttpStatusCode.Created);
    }
}