using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("leaderboard")]
    [Authentication(false)]
    public ApiLeaderboardEntryWrapper? GetLeaderboard(RequestContext context, RealmDatabaseContext database, string id)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        bool descending = bool.Parse(context.QueryString["descending"] ?? "false");

        bool onlyShowBest = bool.Parse(context.QueryString["onlyBest"] ?? "true");
        bool completed = bool.Parse(context.QueryString["completed"] ?? "true");

        string? onLevel = context.QueryString["level"];
        string? byUser = context.QueryString["byUser"];
        
        string? orderString = context.QueryString["orderBy"];

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries();

        LeaderboardOrderType order = orderString switch
        {
            "score" => LeaderboardOrderType.Score,
            "playTime" => LeaderboardOrderType.PlayTime,
            "tokens" => LeaderboardOrderType.Tokens,
            "date" => LeaderboardOrderType.Date,
            _ => LeaderboardOrderType.Score
        };

        return new ApiLeaderboardEntryWrapper(entries, from, count, order, descending, onlyShowBest, completed, onLevel, byUser);
    }
}