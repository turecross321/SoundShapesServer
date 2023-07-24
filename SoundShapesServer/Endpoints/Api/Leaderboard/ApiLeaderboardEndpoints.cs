using AttribDoc.Attributes;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Attributes;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Leaderboard;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard"), Authentication(false)]
    [DocUsesPageData]
    [DocSummary("Retrieves leaderboard.")]
    public ApiLeaderboardEntriesWrapper GetLeaderboard(RequestContext context, GameDatabaseContext database, string id)
    {
        (int from, int count, bool descending) = PaginationHelper.GetPageData(context);

        string? onLevel = context.QueryString["onLevel"];        
        string? byUserId = context.QueryString["byUser"];
        bool onlyBest = bool.Parse(context.QueryString["onlyBest"] ?? "false");
        bool? completed = null;
        if (bool.TryParse(context.QueryString["completed"], out bool completedTemp)) completed = completedTemp;

        GameUser? byUser = null;
        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        
        string? orderString = context.QueryString["orderBy"];

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "notes" => LeaderboardOrderType.Notes,
            "creationDate" => LeaderboardOrderType.CreationDate,
            _ => LeaderboardOrderType.Score
        };

        LeaderboardFilters filters = new (onLevel, byUser, completed, onlyBest);
        (IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries) =
            database.GetLeaderboardEntries(order, descending, filters, from, count);

        return new ApiLeaderboardEntriesWrapper(paginatedEntries, allEntries.Count(), from, descending);
    }
}