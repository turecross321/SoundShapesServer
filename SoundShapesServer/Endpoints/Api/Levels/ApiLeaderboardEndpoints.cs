using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api.Levels;

public class ApiLeaderboardEndpoints : EndpointGroup
{
    [ApiEndpoint("level/{levelId}/leaderboard")]
    [Authentication(false)]
    public ApiLeaderboardEntryWrapper GetLeaderboard(RequestContext context, RealmDatabaseContext database, string levelId)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        
        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries(levelId);

        return LeaderboardHelper.LeaderboardEntriesToApiWrapper(entries, from, count);
    }

    [ApiEndpoint("level/{levelId}/leaderboard/{userId}")]
    [Authentication(false)]
    public ApiLeaderboardEntryResponse? GetLeaderboardEntryByUser(RequestContext context, RealmDatabaseContext database,
        string levelId, string userId)
    {
        GameUser? user = database.GetUserWithId(userId);
        if (user == null) return null;

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries(levelId);

        LeaderboardEntry? entry = entries.FirstOrDefault(e => e.User.Id == user.Id);
        if (entry == null) return null;
        
        int position = entries.Count(e => e.Score < entry.Score);

        return LeaderboardHelper.LeaderboardEntryToApiResponse(entry, position);
    }
}