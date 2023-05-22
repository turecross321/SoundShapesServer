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
    public ApiLevelsWrapper GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];

        string? byUserId = context.QueryString["byUser"];
        string? likedByUserId = context.QueryString["likedBy"];
        string? completedByString = context.QueryString["completedBy"];
        string? inAlbumId = context.QueryString["inAlbum"];
        
        bool? inDaily = null;
        if (TryParse(context.QueryString["inDaily"], out bool inDailyTemp)) inDaily = inDailyTemp;
        string? inDailyDateString = context.QueryString["inDailyDate"];

        bool? lastDate = null;
        if (TryParse(context.QueryString["inLastDaily"], out bool lastDateTemp)) lastDate = lastDateTemp;

        string? searchQuery = context.QueryString["search"];
        
        string? bpmString = context.QueryString["bpm"];
        string? transposeValueString = context.QueryString["transposeValue"];
        string? scaleString = context.QueryString["scaleIndex"];

        GameUser? byUser = null;
        GameUser? likedBy = null;
        GameUser? completedBy = null;
        GameAlbum? inAlbum = null;
        DateTimeOffset? inDailyDate = null;
        int? bpm = null;
        LevelMusicScale? scale = null;
        int? transposeValue = null;

        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        if (likedByUserId != null) likedBy = database.GetUserWithId(likedByUserId);
        if (completedByString != null) completedBy = database.GetUserWithId(completedByString);
        if (inAlbumId != null) inAlbum = database.GetAlbumWithId(inAlbumId);
        if (inDailyDateString != null) inDailyDate = DateTimeOffset.Parse(inDailyDateString);
        if (bpmString != null) bpm = int.Parse(bpmString);
        if (scaleString != null) scale = Enum.Parse<LevelMusicScale>(scaleString);
        if (transposeValueString != null) transposeValue = int.Parse(transposeValueString);

        LevelFilters filters = new (byUser, likedBy, inAlbum, inDaily, inDailyDate, lastDate, 
            searchQuery, completedBy, bpm, scale, transposeValue);

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
            "totalDeaths" => LevelOrderType.TotalDeaths,
            "totalPlayTime" => LevelOrderType.TotalPlayTime,
            "averagePlayTime" => LevelOrderType.AveragePlayTime,
            "totalScreens" => LevelOrderType.TotalScreens,
            "totalEntities" => LevelOrderType.TotalEntities,
            "bpm" => LevelOrderType.Bpm,
            "transposeValue" => LevelOrderType.TransposeValue,
            _ => LevelOrderType.DoNotOrder
        };

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
    
    [ApiEndpoint("levels/id/{id}/edit", Method.Post)]
    public Response EditLevel(RequestContext context, GameDatabaseContext database, GameUser user,
        ApiEditLevelRequest body, string id)
    {
        if (body == null) throw new ArgumentNullException(nameof(body));

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (PermissionHelper.IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }
        
        GameLevel publishedLevel = database.EditLevel(new PublishLevelRequest(body), level);
        return new Response(new ApiLevelFullResponse(publishedLevel), ContentType.Json, HttpStatusCode.Created);
    }
    
    [ApiEndpoint("levels/id/{id}/remove", Method.Post)]
    public Response RemoveLevel(RequestContext context, GameDatabaseContext database, IDataStore dataStore, GameUser user, string id)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id != user.Id)
        {
            if (PermissionHelper.IsUserModeratorOrMore(user) == false)
                return HttpStatusCode.Unauthorized;
        }

        database.RemoveLevel(level, dataStore);
        return HttpStatusCode.OK;
    }

    [ApiEndpoint("levels/id/{levelId}/users/id/{userId}")]
    [Authentication(false)]
    public Response HasUserCompletedLevel(RequestContext context, GameDatabaseContext database, string levelId, string userId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return HttpStatusCode.NotFound;
        
        bool completed = level.UniqueCompletions.Contains(user);

        return new Response(new ApiHasUserCompletedLevelResponse(completed), ContentType.Json);
    }
}