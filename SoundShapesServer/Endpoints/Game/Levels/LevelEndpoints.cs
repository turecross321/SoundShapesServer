using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page", ContentType.Json)]
    [GameEndpoint("~index:level.page", ContentType.Json)]
    public LevelsWrapper? LevelsEndpoint(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        string? category = context.QueryString["search"];
        string? order = context.QueryString["order"];
        string? type = context.QueryString["type"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? decorated = context.QueryString["decorated"];

        if (query != null && query.Contains("author.id:"))
            return LevelsByUser(user, query, database, from, count);
        
        if (query != null && query.Contains("metadata.displayName:"))
            return SearchForLevels(user, query, database, from, count);

        return category switch
        {
            "tagged3" => database.DailyLevels(user, from, count),
            "greatesthits" => database.GreatestHits(user, from, count),
            "newest" => database.NewestLevels(user, from, count),
            "top" => database.TopLevels(user, from, count),
            "random" => database.RandomLevels(user, from, count),
            "largest" => database.LargestLevels(user, from, count),
            _ => null
        };
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

        return database.GetUsersLikedLevels(user, userToGetLevelsFrom, from, count);
    }
    
    private LevelsWrapper? LevelsByUser(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string id = query.Split(":")[2];

        GameUser? usersToGetLevelsFrom = database.GetUserWithId(id);

        if (usersToGetLevelsFrom == null) return null;

        return database.GetLevelsPublishedByUser(user, usersToGetLevelsFrom, from, count);
    }

    private LevelsWrapper? SearchForLevels(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string levelName = query.Split(":")[1];

        return database.SearchForLevels(user, levelName, from, count);
    }
}