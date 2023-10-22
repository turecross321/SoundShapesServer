using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.RequestContextExtensions;
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

        LevelOrderType? order = null;
        LevelFilters? filters = null;
        
        const string searchQuery = "metadata.displayName:";
        const string byUserQuery = "author.id:/~identity:";
        
        // Daily levels should be the only levels type that works for unauthenticated users. This is because it's required to show the EULA.
        if (token is not { TokenType: TokenType.GameAccess } && searchString != "tagged3")
            return null;
        
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
                filters = new LevelFilters
                {
                    CreatedAfter = DateTimeOffset.UtcNow.AddDays(-7)
                };
                order = LevelOrderType.UniquePlays;
                break;
        }
        
        filters ??= context.GetLevelFilters(database);
        order ??= context.GetLevelOrderType();

        (GameLevel[] levels, int totalLevels) = database.GetPaginatedLevels((LevelOrderType)order, descending, filters, from, count, user);

        return new ListResponse<LevelResponse>(levels.Select(l=>new LevelResponse(l, user)), totalLevels, from, count);
    }
}