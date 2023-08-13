using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Game;

public class LeaderboardEndpoints : EndpointGroup
{
    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.post", Method.Post)]
    [GameEndpoint("~identity:{userId}/~record:%2F~level%3A{arguments}", Method.Post)]
    public Response SubmitScore(RequestContext context, GameDatabaseContext database, GameUser user, string userId, string? arguments, string body, string? levelId)
    {
        if (arguments != null)
        {
            string[] args = arguments.Split('.');
            levelId = args[0];
            string requestType = args[1];

            if (requestType != "post") return HttpStatusCode.NotFound;
        }

        if (levelId == null) return HttpStatusCode.NotFound;

        LeaderboardSubmissionRequest deSerializedRequest = DeSerializeSubmission(body);
        
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;
        
        if (deSerializedRequest.Completed) database.AddCompletionToLevel(user, level);
        database.CreatePlay(user, level);
        database.AddDeathsToLevel(user, level, deSerializedRequest.Deaths);
        database.SetLevelDifficulty(level);

        database.CreateLeaderboardEntry(deSerializedRequest, user, level);

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page")] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.page")] // community levels
    [GameEndpoint("{levelId}/~leaderboard.page")] // recent activity community levels 
    public ListResponse<LeaderboardEntryResponse>? GetLeaderboard(RequestContext context, GameDatabaseContext database, string levelId)
    {
        (int from, int count, bool _) = PaginationHelper.GetPageData(context);
        const bool descending = false;

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        LeaderboardFilters filters = new (level, onlyBest: true, completed: true);
        (int totalEntries, LeaderboardEntry[] paginatedEntries) = database.GetPaginatedLeaderboardEntries(LeaderboardOrderType.Score, descending, filters, from, count);

        List<LeaderboardEntryResponse> responses = 
            paginatedEntries.Select((t, i) => 
                new LeaderboardEntryResponse(t, CalculateEntryPlacement(totalEntries, from, i, descending, false))).ToList();

        return new ListResponse<LeaderboardEntryResponse>(responses, totalEntries, from, count);
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.near")] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.near")] // community levels
    [GameEndpoint("{levelId}/~leaderboard.near")] // recent activity community levels 
    public LeaderboardEntryResponse[]? GetLeaderboardNearPlayer(RequestContext context, GameDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;
        
        LeaderboardFilters filters = new (level, user, completed:true);

        (int _, LeaderboardEntry[] paginatedEntries) = database.GetPaginatedLeaderboardEntries(LeaderboardOrderType.Score, false, filters, 0, 1);
        
        return paginatedEntries.Select(e=> new LeaderboardEntryResponse(e, database.GetLeaderboardEntryPosition(e) + 1)).ToArray();
    }
}