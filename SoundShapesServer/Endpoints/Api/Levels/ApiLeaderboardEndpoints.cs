using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types;

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
}