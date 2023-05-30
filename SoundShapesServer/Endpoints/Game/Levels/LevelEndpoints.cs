using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page", ContentType.Json)]
    [GameEndpoint("~index:level.page", ContentType.Json)]
    [Authentication(false)]
    public Response LevelsEndpoint(RequestContext context, GameDatabaseContext database, GameUser? user, GameSession? session)
    {
        string? orderString = context.QueryString["orderBy"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string searchString = context.QueryString["search"] ?? "all";
        bool descending = bool.Parse(context.QueryString["descending"] ?? "true");
        
        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (session == null || user == null)
        {
            if (searchString == "tagged3") return new Response(new LevelsWrapper(), ContentType.Json);
            return HttpStatusCode.Forbidden;
        }
        if (session.SessionType != (int)SessionType.Game)
        {
            if (session.SessionType != (int)SessionType.Unauthorized || searchString != "tagged3")
                return HttpStatusCode.Forbidden;
        }

        LevelOrderType? order = null;
        LevelFilters? filters = null;

        if (query != null && query.Contains("author.id:"))
        {
            string id = query.Split(":")[2];

            GameUser? usersToGetLevelsFrom = database.GetUserWithId(id);
            if (usersToGetLevelsFrom == null) return HttpStatusCode.NotFound;

            filters = new LevelFilters(usersToGetLevelsFrom);
        }

        else if (query != null && query.Contains("metadata.displayName:"))
        {
            filters = new LevelFilters(search: query.Split(":")[1]);
        }
        
        else switch (searchString)
        {
            case "tagged3":
                filters = new LevelFilters(inDaily:true, inLatestDaily:true);
                order = LevelOrderType.UniquePlays;
                break;
            // ReSharper disable once StringLiteralTypo
            case "greatesthits":
                filters = new LevelFilters();
                order = LevelOrderType.Relevance;
                break;
        }

        order ??= LevelHelper.GetLevelOrderType(orderString);
        filters ??= LevelHelper.GetLevelFilters(context, database);

        (GameLevel[] levels, int totalLevels) = database.GetLevels((LevelOrderType)order, descending, filters, from, count);

        return new Response(new LevelsWrapper(levels, user, totalLevels, from, count), ContentType.Json);
    }
    
    [GameEndpoint("~identity:{userId}/~queued:*.page", ContentType.Json)]
    [GameEndpoint("~identity:{userId}/~like:*.page", ContentType.Json)]
    public RelationLevelsWrapper? QueuedAndLiked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) return null;
        
        (GameLevel[] levels, int totalLevels) = database.GetLevels(LevelOrderType.DoNotOrder, true, new LevelFilters(likedOrQueuedByUser: userToGetLevelsFrom), from, count);
        
        return new RelationLevelsWrapper(levels, user, totalLevels, from, count);
    }
}