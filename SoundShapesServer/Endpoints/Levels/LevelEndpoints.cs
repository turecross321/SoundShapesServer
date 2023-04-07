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
    [Authentication(false)]
    public LevelResponsesWrapper?LevelsEndpoint(RequestContext context, RealmDatabaseContext database)
    {
        string? category = context.QueryString["search"];
        string? order = context.QueryString["order"];
        string? type = context.QueryString["type"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? decorated = context.QueryString["decorated"];

        if (query != null && query.Contains("author.id:")) // Levels by player
            return LevelsByUser(query, database, from, count);
        else if (query != null && query.Contains("metadata.displayName:")) // Search
            return SearchForLevels(query, database, from, count);

        return null;
    }
    
    [Endpoint("/otg/~identity:{userId}/~queued:*.page", ContentType.Json)]
    [Endpoint("/otg/~identity:{userId}/~like:*.page", ContentType.Json)]
    public LevelResponsesWrapper? QueuedAndLiked(RequestContext context, RealmDatabaseContext database, string userId)
    {
        // Queued levels and Liked levels should be two different categories, but there aren't any buttons seperating them, so i'm just not going to implement them as one for now
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        GameUser? user = database.GetUserWithId(userId);

        if (user == null) return null;

        return database.GetUsersLikedLevels(user, from, count);
    }
    
    private LevelResponsesWrapper? LevelsByUser(string query, RealmDatabaseContext database, int from, int count)
    {
        string id = query.Split(":")[2];

        GameUser? user = database.GetUserWithId(id);

        if (user == null) return null;

        return database.GetLevelsPublishedByUser(user, from, count);
    }

    private LevelResponsesWrapper? SearchForLevels(string query, RealmDatabaseContext database, int from, int count)
    {
        string levelName = query.Split(":")[1];

        return database.SearchForLevels(levelName, from, count);
    }
}