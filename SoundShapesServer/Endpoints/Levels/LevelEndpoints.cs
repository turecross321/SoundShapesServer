using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
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
    // Called from LeaderboardEndpoints
    public bool AddPlay(RealmDatabaseContext database, GameLevel level)
    {
        return database.AddPlayToLevel(level);
    }

    // Called from LevelResourcesEndpoint
    public bool AddUniquePlay(RealmDatabaseContext database, GameUser user, GameLevel level)
    {
        return database.AddUniquePlayToLevel(user, level);
    }
    private LevelResponsesWrapper GetLevels(IEnumerable<GameLevel>? levels, int count, int from, RealmDatabaseContext database)
    {
        int? nextToken;
        
        if (levels.Count() <= count + from) nextToken = null;
        else nextToken = count + from;

        int? previousToken;
        if (from > 0) previousToken = from - 1;
        else previousToken = null;
        
        List<GameLevel> levelList = levels.Skip(from).Take(count).ToList();

        LevelResponse[] levelResponses = new LevelResponse[Math.Min(count, levelList.Count())];

        for (int i = 0; i < levelList.Count; i++)
        {
            levelResponses[i] = LevelHelper.ConvertGameLevelToLevelResponse(levelList[i]);;
        }

        LevelResponsesWrapper response = new()
        {
            items = levelResponses,
            count = levelResponses.Length,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return response;
    }
    
    [Endpoint("/otg/~index:*.page", ContentType.Json)]
    [Endpoint("/otg/~index:level.page", ContentType.Json)]
    [Authentication(false)]
    public LevelResponsesWrapper LevelsEndpoint(RequestContext context, RealmDatabaseContext database)
    {
        var category = context.QueryString["search"];
        var order = context.QueryString["order"];
        var type = context.QueryString["type"];
        var query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        var decorated = context.QueryString["decorated"];

        IEnumerable<GameLevel>? levels = new List<GameLevel>();
        
        if (query != null && query.Contains("author.id:")) // Levels by player
            levels = LevelsByUser(query, database);
        else if (query != null && query.Contains("metadata.displayName:")) // Search
            levels = SearchForLevels(query, database);

        return GetLevels(levels, count, from, database);
    }
    
    [Endpoint("/otg/~identity:{userId}/~queued:*.page", ContentType.Json)]
    [Endpoint("/otg/~identity:{userId}/~like:*.page", ContentType.Json)]
    public LevelResponsesWrapper? QueuedAndLiked(RequestContext context, RealmDatabaseContext database, string userId)
    {
        // Queued levels and Liked levels should be two different categories, but there aren't any buttons seperating the two, so i'm just not going to implement it for now
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        GameUser? user = database.GetUserWithId(userId);

        if (user == null) return null;

        IEnumerable<GameLevel> levels = database.GetUsersLikedLevels(user);
        
        return GetLevels(levels, count, from, database);
    }
    
    private IEnumerable<GameLevel>? LevelsByUser(string query, RealmDatabaseContext database)
    {
        string id = query.Split(":")[2];

        GameUser? user = database.GetUserWithId(id);
        
        if (user == null) return null;

        IEnumerable<GameLevel> levels = database.GetLevelsPublishedByUser(user);

        return levels;
    }

    private IEnumerable<GameLevel> SearchForLevels(string query, RealmDatabaseContext database)
    {
        string levelName = query.Split(":")[1];

        return database.SearchForLevels(levelName);
    }
}