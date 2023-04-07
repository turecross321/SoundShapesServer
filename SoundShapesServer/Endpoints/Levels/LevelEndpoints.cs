using System.Runtime.CompilerServices;
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
    public bool AddCompletion(RealmDatabaseContext database, GameLevel level, GameUser user)
    {
        return database.AddUserToLevelCompletions(level, user);
    }
    // Called from LeaderboardEndpoints
    public bool AddPlay(RealmDatabaseContext database, GameLevel level)
    {
        return database.AddPlayToLevel(level);
    }

    // Called from LeaderboardEndpoints
    public bool AddUniquePlay(RealmDatabaseContext database, GameUser user, GameLevel level)
    {
        return database.AddUniquePlayToLevel(level, user);
    }
    private LevelResponsesWrapper GetLevels(GameLevel[] levels, int totalLevels, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalLevels, from, count);
        
        LevelResponse[] levelResponses = new LevelResponse[levels.Length];

        for (int i = 0; i < levels.Length; i++)
        {
            levelResponses[i] = LevelHelper.ConvertGameLevelToLevelResponse(levels[i]);
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
        string? category = context.QueryString["search"];
        string? order = context.QueryString["order"];
        string? type = context.QueryString["type"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? decorated = context.QueryString["decorated"];

        GameLevel[] levels = Array.Empty<GameLevel>();
        int totalLevels = 0;
        
        if (query != null && query.Contains("author.id:")) // Levels by player
            (levels, totalLevels) = LevelsByUser(query, database, from, count);
        else if (query != null && query.Contains("metadata.displayName:")) // Search
            (levels, totalLevels) = SearchForLevels(query, database, from, count);

        return GetLevels(levels, totalLevels, from, count);
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

        (GameLevel[] levels, int totalLevels) = database.GetUsersLikedLevels(user, from, count);
        
        return GetLevels(levels, totalLevels, from, count);
    }
    
    private (GameLevel[], int) LevelsByUser(string query, RealmDatabaseContext database, int from, int count)
    {
        string id = query.Split(":")[2];

        GameUser? user = database.GetUserWithId(id);

        if (user == null) return (Array.Empty<GameLevel>(), 0);

        return database.GetLevelsPublishedByUser(user, from, count);
    }

    private (GameLevel[], int) SearchForLevels(string query, RealmDatabaseContext database, int from, int count)
    {
        string levelName = query.Split(":")[1];

        return database.SearchForLevels(levelName, from, count);
    }
}