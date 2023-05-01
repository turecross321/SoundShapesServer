using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
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
        string? album = context.QueryString["album"];

        IQueryable<GameLevel>? levels = category switch
        {
            "daily" => database.GetDailyLevels(DateTimeOffset.UtcNow),
            _ => null
        };

        levels ??= database.GetLevels();
        
        if (byUser != null)
        {
            GameUser? userToGetLevelsFrom = database.GetUserWithId(byUser);
            if (userToGetLevelsFrom == null) return null;

            levels = levels
                .AsEnumerable()
                .Where(l => l.Author.Id == byUser)
                .AsQueryable();
        }
        if (likedByUser != null)
        {
            GameUser? userToGetLevelsFrom = database.GetUserWithId(likedByUser);
            if (userToGetLevelsFrom == null) return null;

            levels = levels
                    .AsEnumerable()
                    .Where(l => userToGetLevelsFrom.LikedLevels
                    .Select(relation => relation.Level.Id)
                    .Contains(l.Id))
                    .AsQueryable();
        }

        if (album != null)
        {
            GameAlbum? albumToGetLevelsFrom = database.GetAlbumWithId(album);
            if (albumToGetLevelsFrom == null) return null;

            levels = levels
                .AsEnumerable()
                .Where(l => albumToGetLevelsFrom.Levels.Contains(l))
                .AsQueryable();
        }
        
        // todo but this in helper maybge=?

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