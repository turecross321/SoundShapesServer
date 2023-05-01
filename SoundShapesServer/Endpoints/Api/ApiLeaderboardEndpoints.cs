using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("level/{id}/leaderboard")]
    [Authentication(false)]
    public ApiLeaderboardEntryWrapper GetLeaderboard(RequestContext context, RealmDatabaseContext database, string id)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntriesOnLevel(id);

        return new ApiLeaderboardEntryWrapper(entries, from, count);
    }

    [ApiEndpoint("level/{levelId}/leaderboard/{userId}")]
    [Authentication(false)]
    public ApiLeaderboardEntryResponse? GetLeaderboardEntryByUser(RequestContext context, RealmDatabaseContext database,
        string levelId, string userId)
    {
        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntriesOnLevel(levelId);

        LeaderboardEntry? bestEntry = GetBestEntry(entries, user);
        return bestEntry == null ? null : new ApiLeaderboardEntryResponse(bestEntry, GetEntryPlacement(entries, bestEntry));
    }
}