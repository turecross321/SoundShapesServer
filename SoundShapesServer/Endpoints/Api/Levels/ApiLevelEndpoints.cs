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
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    public ApiLevelResponseWrapper GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? byUserId = context.QueryString["byUser"];
        string? likedByUserId = context.QueryString["likedByUser"];
        string? inAlbumId = context.QueryString["inAlbum"];
        string? inDailyString = context.QueryString["inDaily"];

        string? searchQuery = context.QueryString["search"];
        
        bool? completed = null;
        if (TryParse(context.QueryString["completed"], out bool completedTemp)) completed = completedTemp;

        GameUser? byUser = null;
        GameUser? likedByUser = null;
        GameAlbum? inAlbum = null; 
        DateTimeOffset? inDaily = null;

        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        if (likedByUserId != null) likedByUser = database.GetUserWithId(likedByUserId);
        if (inAlbumId != null) inAlbum = database.GetAlbumWithId(inAlbumId);
        if (inDailyString != null) inDaily = DateTimeOffset.Parse(inDailyString);

        LevelFilters filters = new (byUser, likedByUser, inAlbum, inDaily, searchQuery, completed);

        LevelOrderType order = orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "plays" => LevelOrderType.Plays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "likes" => LevelOrderType.Likes,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "relevance" => LevelOrderType.Relevance,
            "random" => LevelOrderType.Random,
            "deaths" => LevelOrderType.Deaths,
            _ => LevelOrderType.DoNotOrder
        };

        (GameLevel[] levels, int levelCount) = database.GetLevels(user, order, descending, filters, from, count);
        
        return new ApiLevelResponseWrapper(levels, levelCount);
    }

    [ApiEndpoint("levels/{levelId}")]
    [Authentication(false)]
    public ApiLevelFullResponse? Level(RequestContext context, GameDatabaseContext database, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level);
    }
    
    [ApiEndpoint("levels/{id}/edit", Method.Post)]
    public Response EditLevel(RequestContext context, GameDatabaseContext database, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author?.Id != user.Id)
        {
            if (PermissionHelper.IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }
        
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("levels/{id}/remove", Method.Post)]
    public Response RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author?.Id != user.Id)
        {
            if (PermissionHelper.IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }

        database.RemoveLevel(level, dataStore);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("levels/{id}/completed")]
    public Response HasUserCompletedLevel(RequestContext context, GameDatabaseContext database, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;
        
        bool completed = level.UniqueCompletions.Contains(user);

        return new Response(new ApiHasUserCompletedLevelResponse(completed), ContentType.Json);
    }
}