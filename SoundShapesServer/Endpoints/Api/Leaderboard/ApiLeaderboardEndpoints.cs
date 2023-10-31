using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Extensions;
using SoundShapesServer.Extensions.RequestContextExtensions;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{levelId}/leaderboard"), Authentication(false)]
    [DocUsesPageData]
    [DocUsesFilter<LeaderboardFilters>]
    [DocSummary("Retrieves leaderboard of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiListResponse<ApiLeaderboardEntryResponse> GetLeaderboard(RequestContext context, GameDatabaseContext database, string levelId, GameUser? user)
    {
        (int from, int count, bool descending) = context.GetPageData(false);

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;
        
        string? orderString = context.QueryString["orderBy"];
        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "notes" => LeaderboardOrderType.Notes,
            "creationDate" => LeaderboardOrderType.CreationDate,
            _ => LeaderboardOrderType.Score
        };

        LeaderboardFilters filters = context.GetFilters<LeaderboardFilters>(database);
        (LeaderboardEntry[] paginatedEntries, int totalEntries) = database.GetPaginatedLeaderboardEntries(level, order, descending, filters, from, count, user);

        return new ApiListResponse<ApiLeaderboardEntryResponse>(paginatedEntries.Select(e =>
            new ApiLeaderboardEntryResponse(e, order, filters)), totalEntries);
    }
}