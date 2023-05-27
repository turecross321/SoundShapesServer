using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    public ApiLevelsWrapper GetLevels(RequestContext context, GameDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        
        string? orderString = context.QueryString["orderBy"];

        LevelOrderType order = LevelHelper.GetLevelOrderType(orderString);
        LevelFilters filters = LevelHelper.GetLevelFilters(context, database);

        (GameLevel[] levels, int levelCount) = database.GetLevels(order, descending, filters, from, count);
        
        return new ApiLevelsWrapper(levels, levelCount);
    }

    [ApiEndpoint("levels/id/{levelId}")]
    [Authentication(false)]
    public ApiLevelFullResponse? GetLevelWithId(RequestContext context, GameDatabaseContext database, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level);
    }
}