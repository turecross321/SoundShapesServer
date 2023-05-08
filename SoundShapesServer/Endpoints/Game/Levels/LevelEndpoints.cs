using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page", ContentType.Json)]
    [GameEndpoint("~index:level.page", ContentType.Json)]
    [Authentication(false)]
    public LevelsWrapper? LevelsEndpoint(RequestContext context, GameDatabaseContext database, GameUser? user)
    {
        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (user == null) return new LevelsWrapper(); 
        
        string? orderString = context.QueryString["orderBy"];
        string? query = context.QueryString["query"];
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string categoryString = context.QueryString["search"] ?? "all";
        
        LevelOrderType? order = null;
        LevelFilters filters = new ();

        if (query != null && query.Contains("author.id:"))
        {
            string id = query.Split(":")[2];

            GameUser? usersToGetLevelsFrom = database.GetUserWithId(id);
            if (usersToGetLevelsFrom == null) return null;

            filters = new LevelFilters(usersToGetLevelsFrom);
            order = LevelOrderType.CreationDate;
        }

        else if (query != null && query.Contains("metadata.displayName:"))
        {
            filters = new LevelFilters(search: query.Split(":")[1]);
            order = LevelOrderType.Relevance;
        }
        
        else switch (categoryString)
        {
            case "tagged3":
                filters = new LevelFilters(inDailyDate: DateTimeOffset.UtcNow);
                order = LevelOrderType.UniquePlays;
                break;
            // ReSharper disable once StringLiteralTypo
            case "greatesthits":
                order = LevelOrderType.Relevance;
                break;
        }
        
        order ??= orderString switch
        {
            "creationDate" => LevelOrderType.CreationDate,
            "uniquePlays" => LevelOrderType.UniquePlays,
            "random" => LevelOrderType.Random,
            "fileSize" => LevelOrderType.FileSize,
            "difficulty" => LevelOrderType.Difficulty,
            "likes" => LevelOrderType.Likes,
            _ => LevelOrderType.CreationDate
        };

        (GameLevel[] levels, int totalLevels) = database.GetLevels(user, (LevelOrderType)order, true, filters, from, count);

        return new LevelsWrapper(levels, user, totalLevels, from, count);
    }
    
    [GameEndpoint("~identity:{userId}/~queued:*.page", ContentType.Json)]
    [GameEndpoint("~identity:{userId}/~like:*.page", ContentType.Json)]
    public LevelsWrapper? QueuedAndLiked(RequestContext context, GameDatabaseContext database, GameUser user, string userId)
    {
        // Queued levels and Liked levels should be two different categories, but there aren't any buttons seperating them, so i'm just not going to implement them as one for now
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        GameUser? userToGetLevelsFrom = database.GetUserWithId(userId);
        if (userToGetLevelsFrom == null) return null;

        (GameLevel[] levels, int totalLevels) = database.GetLevels(user, LevelOrderType.DoNotOrder, true, new LevelFilters(likedByUser: userToGetLevelsFrom), from, count);

        return new LevelsWrapper(levels, user, totalLevels, from, count);
    }
}