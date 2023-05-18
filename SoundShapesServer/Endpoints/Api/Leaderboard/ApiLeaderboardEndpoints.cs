using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Users;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api.Leaderboard;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard")]
    [Authentication(false)]
    public ApiLeaderboardEntryWrapper GetLeaderboard(RequestContext context, GameDatabaseContext database, string id)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = Parse(context.QueryString["descending"] ?? "false");
        
        string? onLevel = context.QueryString["onLevel"];        
        string? byUserId = context.QueryString["byUser"];
        bool onlyBest = Parse(context.QueryString["onlyBest"] ?? "false");
        bool? completed = null;
        if (TryParse(context.QueryString["completed"], out bool completedTemp)) completed = completedTemp;

        GameUser? byUser = null;
        if (byUserId != null) byUser = database.GetUserWithId(byUserId);
        
        string? orderString = context.QueryString["orderBy"];

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "tokenCount" => LeaderboardOrderType.TokenCount,
            "date" => LeaderboardOrderType.Date,
            _ => LeaderboardOrderType.Score
        };

        LeaderboardFilters filters = new (onLevel, byUser, completed, onlyBest);
        (IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries) =
            database.GetLeaderboardEntries(order, descending, filters, from, count);

        return new ApiLeaderboardEntryWrapper(paginatedEntries, allEntries.Count(), from, descending);
    }
}