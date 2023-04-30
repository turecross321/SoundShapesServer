using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page", ContentType.Json)]
    [GameEndpoint("~index:level.page", ContentType.Json)]
    [Authentication(false)]
    public LevelsWrapper? LevelsEndpoint(RequestContext context, RealmDatabaseContext database, GameUser? user)
    {
        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (user == null) return new LevelsWrapper(); 
        
        string? category = context.QueryString["search"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        if (query != null && query.Contains("author.id:"))
            return LevelsByUser(user, query, database, from, count);
        
        if (query != null && query.Contains("metadata.displayName:"))
            return SearchForLevels(user, query, database, from, count);

        IQueryable<GameLevel>? levels = category switch
        {
            "tagged3" => database.DailyLevels(DateTimeOffset.UtcNow),
            "greatesthits" => database.GreatestHits(),
            "newest" => database.NewestLevels(),
            "top" => database.TopLevels(),
            "random" => database.RandomLevels(),
            "largest" => database.LargestLevels(),
            "hardest" => database.HardestLevels(),
            _ => null
        };

        return levels == null ? null : new LevelsWrapper(levels, user, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~queued:*.page", ContentType.Json)]
    [GameEndpoint("~identity:{userId}/~like:*.page", ContentType.Json)]
    public LevelsWrapper? QueuedAndLiked(RequestContext context, RealmDatabaseContext database, GameUser user, string userId)
    {
        // Queued levels and Liked levels should be two different categories, but there aren't any buttons seperating them, so i'm just not going to implement them as one for now
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);

        if (userToGetLevelsFrom == null) return null;

        IQueryable<GameLevel> levels = database.GetUsersLikedLevels(userToGetLevelsFrom);
        
        return new LevelsWrapper(levels, user, from, count);
    }
    
    private LevelsWrapper? LevelsByUser(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string id = query.Split(":")[2];

        GameUser? usersToGetLevelsFrom = database.GetUserWithId(id);

        if (usersToGetLevelsFrom == null) return null;

        IQueryable<GameLevel> levels = database.GetLevelsPublishedByUser(usersToGetLevelsFrom);

        return new LevelsWrapper(levels, user, from, count);
    }

    private LevelsWrapper SearchForLevels(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string levelName = query.Split(":")[1];

        IQueryable<GameLevel> levels = database.SearchForLevels(levelName);
        return new LevelsWrapper(levels, user, from, count);
    }
}