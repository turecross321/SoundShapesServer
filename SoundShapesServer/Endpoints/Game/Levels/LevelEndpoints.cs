using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using ContentType = Bunkum.Listener.Protocol.ContentType;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page")]
    [GameEndpoint("~index:level.page")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    [Authentication(false)]
    public ListResponse<LevelResponse>? GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user, GameSession? session)
    {
        string? query = context.QueryString["query"];
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);
        string searchString = context.QueryString["search"] ?? "all";

        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (session == null || user == null)
        {
            if (searchString == "tagged3") 
                return new ListResponse<LevelResponse>();
            
            return null;
        }
        if (session.SessionType != SessionType.Game)
        {
            if (session.SessionType != SessionType.GameUnAuthorized || searchString != "tagged3")
                return null;
        }

        LevelOrderType? order = null;
        LevelFilters? filters = null;
        
        const string searchQuery = "metadata.displayName:";
        const string byUserQuery = "author.id:/~identity:";
        
        // search
        if (query != null && query.StartsWith(searchQuery))
        {
            filters = new LevelFilters(search: string.Concat(query.Skip(searchQuery.Length)));
            order = LevelOrderType.UniquePlays;
        }
        
        // published by user
        else if (query != null && query.StartsWith(byUserQuery))
        {
            string byUserId = string.Concat(query.Skip(byUserQuery.Length));
            GameUser? usersToGetLevelsFrom = database.GetUserWithId(byUserId);
            if (usersToGetLevelsFrom == null) return new ListResponse<LevelResponse>();

            filters = new LevelFilters(usersToGetLevelsFrom);
            order = LevelOrderType.CreationDate;
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

        (GameLevel[] levels, int totalLevels) = database.GetPaginatedLevels((LevelOrderType)order, descending, filters, from, count, user);

        return new ListResponse<LevelResponse>(levels.Select(l=>new LevelResponse(l, user)), totalLevels, from, count);
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