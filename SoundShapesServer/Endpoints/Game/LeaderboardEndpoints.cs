using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game.Leaderboards;
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
        if (level != null) // Doing this since story levels can be null
        {
            if (deSerializedRequest.Completed) database.AddCompletionToLevel(user, level);
            database.CreatePlay(user, level);
            database.AddDeathsToLevel(user, level, deSerializedRequest.Deaths);
            database.SetLevelDifficulty(level);
        }

        database.CreateLeaderboardEntry(deSerializedRequest, user, levelId);

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page", ContentType.Json)] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.page", ContentType.Json)] // community levels
    [GameEndpoint("{levelId}/~leaderboard.page", ContentType.Json)] // recent activity community levels 
    public LeaderboardEntriesWrapper GetLeaderboard(RequestContext context, GameDatabaseContext database, string levelId)
    {
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");
        
        const bool descending = false;
        (IQueryable<LeaderboardEntry> allEntries, LeaderboardEntry[] paginatedEntries) = database.GetLeaderboardEntries(LeaderboardOrderType.Score, descending, new LeaderboardFilters(levelId, onlyBest:true, completed:true), from, count);
        return new LeaderboardEntriesWrapper(allEntries, paginatedEntries, from, count, descending);
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.near", ContentType.Json)] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.near", ContentType.Json)] // community levels
    [GameEndpoint("{levelId}/~leaderboard.near", ContentType.Json)] // recent activity community levels 
    public LeaderboardEntryResponse[] GetLeaderboardNearPlayer(RequestContext context, GameDatabaseContext database, GameUser user, string levelId)
    {
        LeaderboardFilters filters = new (levelId, user, completed:true);
        
        const bool descending = false;
        (IQueryable<LeaderboardEntry> _, LeaderboardEntry[] paginatedEntries) = database.GetLeaderboardEntries(LeaderboardOrderType.Score, descending, filters, 0, 1);
        
        return paginatedEntries.Select(e=> new LeaderboardEntryResponse(e, database.GetLeaderboardEntryPosition(e) + 1)).ToArray();
    }
}