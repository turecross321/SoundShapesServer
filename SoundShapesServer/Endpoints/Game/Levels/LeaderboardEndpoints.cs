using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game.Leaderboards;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Game.Levels;

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
        }

        if (!database.SubmitScore(deSerializedRequest, user, levelId)) return HttpStatusCode.InternalServerError;

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page", ContentType.Json)]
    [GameEndpoint("~level:{levelId}/~leaderboard.page", ContentType.Json)]
    public LeaderboardEntriesWrapper GetLeaderboard(RequestContext context, RealmDatabaseContext database, string levelId)
    {
        int count = int.Parse(context.QueryString["count"] ?? throw new InvalidOperationException());
        int from = int.Parse(context.QueryString["from"] ?? "0");

        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries(levelId);

        return new LeaderboardEntriesWrapper(entries, from, count);
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.near", ContentType.Json)]
    [GameEndpoint("~level:{levelId}/~leaderboard.near", ContentType.Json)]
    public LeaderboardEntryResponse[] GetLeaderboardByPlayer(RequestContext context, RealmDatabaseContext database, GameUser user, string levelId)
    {
        IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntries(levelId);

        LeaderboardEntry? entry =
            entries.FirstOrDefault(e => e.LevelId == levelId && e.Completed && e.User.Id == user.Id);
        if (entry == null) return Array.Empty<LeaderboardEntryResponse>();
        
        int position = entries.Count(e => e.LevelId == entry.LevelId && e.Score < entry.Score && e.Completed) + 1;

        LeaderboardEntryResponse[] response = new LeaderboardEntryResponse[1];
        response[0] = new LeaderboardEntryResponse(entry, position);
        
        return response;
    }
}