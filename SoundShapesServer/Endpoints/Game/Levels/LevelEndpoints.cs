using System.Net;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
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
    [GameEndpoint("~index:*.page")]
    [GameEndpoint("~index:level.page")]
    [Authentication(false)]
    public Response GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user, GameSession? session)
    {
        string? query = context.QueryString["query"];
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);
        string searchString = context.QueryString["search"] ?? "all";
        
        
        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (session == null || user == null)
        {
            if (searchString == "tagged3") return new Response(new LevelsWrapper(), ContentType.Json);
            return HttpStatusCode.Forbidden;
        }
        if (session.SessionType != SessionType.Game)
        {
            if (session.SessionType != SessionType.GameUnAuthorized || searchString != "tagged3")
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

        filters ??= LevelHelper.GetLevelFilters(context, database);
        order ??= LevelHelper.GetLevelOrderType(context);

        (GameLevel[] levels, int totalLevels) = database.GetLevels((LevelOrderType)order, descending, filters, from, count);

        return new Response(new LevelsWrapper(levels, user, totalLevels, from, count), ContentType.Json);
    }

    [GameEndpoint("~level:{levelId}/~metadata:{args}", ContentType.Plaintext)]
    public string? GetFeaturedLevel(RequestContext context, GameDatabaseContext database, GameUser user, string levelId, string args)
    {
        // Using args in the endpoints here because Bunkum doesn't support . as a separator
        string[] arguments = args.Split('.');
        
        string action = arguments[1];

        if (action != "get") return null;
        
        GameLevel? level = user.FeaturedLevel;
        if (level == null) return null;

        return IdHelper.FormatLevelIdAndVersion(level);
    }
}