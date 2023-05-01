using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLevelEndpoints: EndpointGroup
{
    [ApiEndpoint("levels")]
    [Authentication(false)]
    public ApiLevelResponseWrapper? GetLevels(RequestContext context, RealmDatabaseContext database, GameUser? user)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        string? orderString = context.QueryString["orderBy"];
        
        string category = context.QueryString["category"] ?? "all";
        
        string? byUser = context.QueryString["byUser"];
        string? likedByUser = context.QueryString["likedByUser"];
        string? inAlbum = context.QueryString["inAlbum"];

        IQueryable<GameLevel>? levels = category switch
        {
            "daily" => database.GetDailyLevels(DateTimeOffset.UtcNow),
            _ => null
        };

        levels ??= database.GetLevels();
        levels = LevelHelper.FilterLevels(database, levels, byUser, likedByUser, inAlbum);
        if (levels == null) return null;

        LevelOrderType order = orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "modificationDate" => LevelOrderType.ModificationDate,
            "plays" => LevelOrderType.Plays,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "relevance" => LevelOrderType.Relevance,
            _ => LevelOrderType.CreationDate
        };
        
        return new ApiLevelResponseWrapper(levels, from, count, user, order, descending);
    }

    [ApiEndpoint("level/{levelId}")]
    [Authentication(false)]
    public ApiLevelFullResponse? Level(RequestContext context, RealmDatabaseContext database, string levelId, GameUser? user)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        return level == null ? null : new ApiLevelFullResponse(level, user);
    }
}