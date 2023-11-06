using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses.Leaderboard;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{id}/leaderboard")]
    [Authentication(false)]
    [DocUsesPageData]
    [DocUsesFiltration<LeaderboardFilters>]
    [DocUsesOrder<LeaderboardOrderType>]
    [DocSummary("Retrieves leaderboard of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocRouteParam("id", "Level ID.")]
    public ApiListResponse<ApiLeaderboardEntryResponse> GetLeaderboard(RequestContext context,
        GameDatabaseContext database, string id, GameUser? user)
    {
        (int from, int count, bool descending) = context.GetPageData(false);

        GameLevel? level = database.GetLevelWithId(id);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;

        LeaderboardFilters filters = context.GetFilters<LeaderboardFilters>(database);
        LeaderboardOrderType order = context.GetOrderType<LeaderboardOrderType>() ?? LeaderboardOrderType.Score;

        PaginatedList<LeaderboardEntry> entries =
            database.GetPaginatedLeaderboardEntries(level, order, descending, filters, from, count, user);
        IEnumerable<ApiLeaderboardEntryResponse> responses =
            ApiLeaderboardEntryResponse.FromOldList(entries.Items, order, filters);

        return PaginatedList<ApiLeaderboardEntryResponse>.SwapItems(entries, responses);
    }
}