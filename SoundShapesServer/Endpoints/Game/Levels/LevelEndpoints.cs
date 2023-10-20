using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Responses.Game.Levels;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game.Levels;

public class LevelEndpoints : EndpointGroup
{
    [GameEndpoint("~index:*.page")]
    [GameEndpoint("~index:level.page")]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    [Authentication(false)]
    public ListResponse<LevelResponse>? GetLevels(RequestContext context, GameDatabaseContext database, GameUser? user, GameToken? token)
    {
        string? query = context.QueryString["query"];
        (int from, int count, bool descending) = context.GetPageData();
        string searchString = context.QueryString["search"] ?? "all";

        // Doing this so the game doesn't disconnect for unauthenticated users before getting to the EULA.
        if (token == null || user == null)
        {
            if (searchString == "tagged3") 
                return new ListResponse<LevelResponse>();
            
            return null;
        }
        if (token.TokenType != TokenType.GameAccess)
        {
            if (token.TokenType != TokenType.GameUnAuthorized || searchString != "tagged3")
                return null;
        }

        LevelOrderType? order = null;
        LevelFilters? filters = null;
        
        const string searchQuery = "metadata.displayName:";
        const string byUserQuery = "author.id:/~identity:";
        
        // search
        if (query != null && query.StartsWith(searchQuery))
        {
            filters = new LevelFilters()
            {
                Search = string.Concat(query.Skip(searchQuery.Length))
            };
            
            order = LevelOrderType.UniquePlays;
        }
        
        // published by user
        else if (query != null && query.StartsWith(byUserQuery))
        {
            string byUserId = string.Concat(query.Skip(byUserQuery.Length));
            GameUser? usersToGetLevelsFrom = database.GetUserWithId(byUserId);
            if (usersToGetLevelsFrom == null) return new ListResponse<LevelResponse>();

            filters = new LevelFilters
            {
                ByUser = usersToGetLevelsFrom
            };
            order = LevelOrderType.CreationDate;
        }

        else switch (searchString)
        {
            case "tagged3":
                filters = new LevelFilters
                {
                    InDaily = true,
                    InLatestDaily = true   
                };
                order = LevelOrderType.UniquePlays;
                break;
            // ReSharper disable once StringLiteralTypo
            case "greatesthits":
                filters = new LevelFilters()
                {
                    CreatedAfter = DateTimeOffset.UtcNow.AddDays(-7)
                };
                order = LevelOrderType.UniquePlays;
                break;
        }

        filters ??= LevelHelper.GetLevelFilters(context, database);
        order ??= LevelHelper.GetLevelOrderType(context);

        (GameLevel[] levels, int totalLevels) = database.GetPaginatedLevels((LevelOrderType)order, descending, filters, from, count, user);

        return new ListResponse<LevelResponse>(levels.Select(l=>new LevelResponse(l, user)), totalLevels, from, count);
    }
}