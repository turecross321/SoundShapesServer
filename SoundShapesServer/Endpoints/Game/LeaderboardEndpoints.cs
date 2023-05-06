using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Endpoints.Game.Levels;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game.Leaderboards;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Game;

public class LeaderboardEndpoints : EndpointGroup
{
    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.post", Method.Post)]
    [GameEndpoint("~identity:{userId}/~record:%2F~level%3A{arguments}", Method.Post)]
    public Response SubmitScore(RequestContext context, RealmDatabaseContext database, GameUser user, string userId, string? arguments, string body, string? levelId)
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
            if (deSerializedRequest.Completed) LevelInteractionEndpoints.AddCompletion(database, level, user);
            LevelInteractionEndpoints.AddPlay(database, level);
            LevelInteractionEndpoints.AddUniquePlay(database, level, user);
            LevelInteractionEndpoints.AddDeathsToLevel(database, level, deSerializedRequest.Deaths);
            database.SetLevelDifficulty(level);
        }

        database.SubmitScore(deSerializedRequest, user, levelId);

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page", ContentType.Json)] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.page", ContentType.Json)] // community levels
    [GameEndpoint("{levelId}/~leaderboard.page", ContentType.Json)] // recent activity community levels 
    public LeaderboardEntriesWrapper GetLeaderboard(RequestContext context, RealmDatabaseContext database, string levelId)
    {
        int count = int.Parse(context.QueryString["count"] ?? "9");
        int from = int.Parse(context.QueryString["from"] ?? "0");

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries();
        return new LeaderboardEntriesWrapper(entries, from, count, LeaderboardOrderType.Score, levelId);
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.near", ContentType.Json)] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.near", ContentType.Json)] // community levels
    [GameEndpoint("{levelId}/~leaderboard.near", ContentType.Json)] // recent activity community levels 
    public LeaderboardEntryResponse[]? GetLeaderboardNearPlayer(RequestContext context, RealmDatabaseContext database, GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries();
        IQueryable<LeaderboardEntry> filteredEntries = FilterEntries(entries, levelId, user.Id, true, true);

        return filteredEntries.Select(e=> new LeaderboardEntryResponse(e, GetEntryPlacement(filteredEntries, e) + 1)).ToArray();
    }
}