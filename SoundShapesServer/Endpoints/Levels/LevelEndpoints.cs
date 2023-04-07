using System.Net;
using System.Runtime.CompilerServices;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelEndpoints : EndpointGroup
{
    [Endpoint("/otg/~index:*.page", ContentType.Json)]
    [Endpoint("/otg/~index:level.page", ContentType.Json)]
    public LevelResponsesWrapper? LevelsEndpoint(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        string? category = context.QueryString["search"];
        string? order = context.QueryString["order"];
        string? type = context.QueryString["type"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? decorated = context.QueryString["decorated"];

        if (query != null && query.Contains("author.id:")) // Levels by player
            return LevelsByUser(user, query, database, from, count);
        if (query != null && query.Contains("metadata.displayName:")) // Search
            return SearchForLevels(user, query, database, from, count);
        if (category == "tagged3") // Daily Levels
            return DailyLevels(user, database, from, count);
        if (category == "greatesthits") // Greatest Hits
            return GreatestHits(user, database, from, count);
                
        return null;
    }
    
    [Endpoint("/otg/~identity:{userId}/~queued:*.page", ContentType.Json)]
    [Endpoint("/otg/~identity:{userId}/~like:*.page", ContentType.Json)]
    public LevelResponsesWrapper? QueuedAndLiked(RequestContext context, RealmDatabaseContext database, GameUser user, string userId)
    {
        // Queued levels and Liked levels should be two different categories, but there aren't any buttons seperating them, so i'm just not going to implement them as one for now
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);

        if (userToGetLevelsFrom == null) return null;

        return database.GetUsersLikedLevels(user, userToGetLevelsFrom, from, count);
    }
    
    private LevelResponsesWrapper? LevelsByUser(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string id = query.Split(":")[2];

        GameUser? usersToGetLevelsFrom = database.GetUserWithId(id);

        if (usersToGetLevelsFrom == null) return null;

        return database.GetLevelsPublishedByUser(user, usersToGetLevelsFrom, from, count);
    }

    private LevelResponsesWrapper? SearchForLevels(GameUser user, string query, RealmDatabaseContext database, int from, int count)
    {
        string levelName = query.Split(":")[1];

        return database.SearchForLevels(user, levelName, from, count);
    }

    private LevelResponsesWrapper? DailyLevels(GameUser user, RealmDatabaseContext database, int from, int count)
    {
        return database.DailyLevels(user, from, count);
    }
    
    private LevelResponsesWrapper GreatestHits(GameUser user, RealmDatabaseContext database, int from, int count)
    {
        return database.GreatestHits(user, from, count);
    }
}