using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Leaderboard;
using static System.Boolean;

namespace SoundShapesServer.Endpoints.Api;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard")]
    [Authentication(false)]
    public ApiLeaderboardEntryWrapper GetLeaderboard(RequestContext context, GameDatabaseContext database, string id)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = Parse(context.QueryString["descending"] ?? "false");

        bool onlyBest = Parse(context.QueryString["onlyBest"] ?? "false");
        
        bool? completed = null;
        if (TryParse(context.QueryString["completed"], out bool completedTemp)) completed = completedTemp;

        string? onLevel = context.QueryString["onLevel"];
        string? byUser = context.QueryString["byUser"];
        
        string? orderString = context.QueryString["orderBy"];

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries();

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "tokenCount" => LeaderboardOrderType.TokenCount,
            "date" => LeaderboardOrderType.Date,
            _ => LeaderboardOrderType.Score
        };

        return new ApiLeaderboardEntryWrapper(entries, from, count, order, descending, onlyBest, completed, onLevel, byUser);
    }
}