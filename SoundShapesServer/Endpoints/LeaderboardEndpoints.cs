using System.Data;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests;
using SoundShapesServer.Responses.Leaderboards;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints;

public class LeaderboardEndpoints : EndpointGroup
{
    [Endpoint("/otg/global/~campaign:{levelId}/~leaderboard.post", Method.Post)]
    [Endpoint("/otg/~identity:{userId}/~record:%2F~level%3A{arguments}", Method.Post)]
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

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level != null) // Doing this since story levels can be null
        {
            database.AddPlayToLevel(level);
            database.AddUserToLevelCompletions(level, user);
        }
        
        LeaderboardSubmissionRequest deSerializedRequest = LeaderboardHelper.DeSerializeSubmission(body);

        if (!database.SubmitScore(deSerializedRequest, user, levelId)) return new Response(HttpStatusCode.InternalServerError);

        return new Response(HttpStatusCode.OK);
    }

    [Endpoint("/otg/global/~campaign:{levelId}/~leaderboard.page", ContentType.Json)]
    [Endpoint("/otg/~level:{levelId}/~leaderboard.page", ContentType.Json)]
    public LeaderboardEntriesResponse? GetLeaderboard(RequestContext context, RealmDatabaseContext database, string levelId)
    {
        int count = int.Parse(context.QueryString["count"] ?? throw new InvalidOperationException());
        int from = int.Parse(context.QueryString["from"] ?? "0");

        (LeaderboardEntry[] entries, int totalEntries) = database.GetLeaderboardEntries(levelId, from, count);

        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        LeaderboardEntryResponse[] responseEntries = new LeaderboardEntryResponse[Math.Min(count, entries.Length)];

        for (int i = 0; i < entries.Length; i++)
        {
            responseEntries[i] = new LeaderboardEntryResponse()
            {
                position = from + (i + 1),
                entrant = UserHelper.GetUserResponseFromGameUser(entries[i].user),
                score = entries[i].score
            };
        }

        return new LeaderboardEntriesResponse
        {
            items = responseEntries,
            nextToken = nextToken,
            previousToken = previousToken
        };
    }

    [Endpoint("/otg/global/~campaign:{levelId}/~leaderboard.near", ContentType.Json)]
    [Endpoint("/otg/~level:{levelId}/~leaderboard.near", ContentType.Json)]
    public LeaderboardEntryResponse[] GetLeaderboardNearPlayer(RequestContext context, RealmDatabaseContext database, GameUser user, string levelId)
    {
        LeaderboardEntry? entry = database.GetLeaderboardEntryFromPlayer(user, levelId);
        if (entry == null) return Array.Empty<LeaderboardEntryResponse>();
        
        LeaderboardEntryResponse[] response = new LeaderboardEntryResponse[1];
        response[0] = new LeaderboardEntryResponse()
        {
            position = database.GetPositionOfLeaderboardEntry(entry),
            entrant = UserHelper.GetUserResponseFromGameUser(entry.user),
            score = entry.score
        };

        return response;
    }
}