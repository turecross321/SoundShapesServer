using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("levels/id/{levelId}/leaderboard"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Retrieves leaderboard of level.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    public ApiListResponse<ApiLeaderboardEntryResponse> GetLeaderboard(RequestContext context, GameDatabaseContext database, string levelId, GameUser? user)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context, false);

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return ApiNotFoundError.LevelNotFound;
        
        string? byUserId = context.QueryString["byUser"];
        GameUser? byUser = null;
        if (byUserId != null) byUser = database.GetUserWithId(byUserId);


        bool? obsolete = null;
        if (bool.TryParse(context.QueryString["obsolete"], out bool obsoleteTemp))
            obsolete = obsoleteTemp;
        bool? completed = null;
        if (bool.TryParse(context.QueryString["completed"], out bool completedTemp)) 
            completed = completedTemp;
        
        string? orderString = context.QueryString["orderBy"];

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "notes" => LeaderboardOrderType.Notes,
            "creationDate" => LeaderboardOrderType.CreationDate,
            _ => LeaderboardOrderType.Score
        };

        LeaderboardFilters filters = new (level, byUser, completed, obsolete);
        (int totalEntries, LeaderboardEntry[] paginatedEntries) =
            database.GetPaginatedLeaderboardEntries(order, descending, filters, from, count, user);
        
        List<ApiLeaderboardEntryResponse> responses = 
            paginatedEntries.Select((e, i) => 
                new ApiLeaderboardEntryResponse(e, CalculateEntryPlacement(totalEntries, from, i, descending, true))).ToList();

        return new ApiListResponse<ApiLeaderboardEntryResponse>(responses, totalEntries);
    }
}