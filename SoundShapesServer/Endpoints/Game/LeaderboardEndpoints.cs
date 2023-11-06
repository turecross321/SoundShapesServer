using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class LeaderboardEndpoints : EndpointGroup
{
    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.post", HttpMethods.Post)]
    [GameEndpoint("~identity:{userId}/~record:%2F~level%3A{arguments}", HttpMethods.Post)]
    public Response SubmitScore(RequestContext context, GameDatabaseContext database, GameUser user, GameToken token,
        string userId, string? arguments, string body, string levelId)
    {
        if (arguments != null)
        {
            string[] args = arguments.Split('.');
            levelId = args[0];
            string requestType = args[1];

            if (requestType != "post") return HttpStatusCode.NotFound;
        }

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return HttpStatusCode.NotFound;

        if (!level.HasUserAccess(user))
            return HttpStatusCode.NotFound;

        LeaderboardSubmissionRequest deSerializedRequest = LeaderboardSubmissionRequest.DeSerializeSubmission(body);

        if (deSerializedRequest.Completed)
            database.AddCompletionToLevel(user, level);
        database.CreatePlay(user, level);
        database.AddDeathsToLevel(user, level, deSerializedRequest.Deaths);
        database.SetLevelDifficulty(level);

        database.CreateLeaderboardEntry(deSerializedRequest, user, level, token.PlatformType);

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page")] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.page")] // community levels
    [GameEndpoint("{levelId}/~leaderboard.page")] // recent activity community levels 
    public ListResponse<LeaderboardEntryResponse>? GetLeaderboard(RequestContext context, GameDatabaseContext database,
        GameUser user, string levelId)
    {
        (int from, int count, bool _) = context.GetPageData();

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null)
            return null;

        if (!level.HasUserAccess(user))
            return null;

        LeaderboardOrderType order = LeaderboardOrderType.Score;
        const bool descending = false;
        LeaderboardFilters filters = new() { Completed = true, Obsolete = false };

        PaginatedList<LeaderboardEntry> entries =
            database.GetPaginatedLeaderboardEntries(level, order, descending, filters, from, count, user);
        return new ListResponse<LeaderboardEntryResponse>(
            entries.Items.Select(t => new LeaderboardEntryResponse(t, order, filters)), entries.TotalItems, from,
            count);
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.near")] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.near")] // community levels
    [GameEndpoint("{levelId}/~leaderboard.near")] // recent activity community levels 
    public LeaderboardEntryResponse[]? GetLeaderboardNearPlayer(RequestContext context, GameDatabaseContext database,
        GameUser user, string levelId)
    {
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        if (!level.HasUserAccess(user))
            return null;

        const LeaderboardOrderType order = LeaderboardOrderType.Score;
        const bool descending = false;
        LeaderboardFilters filters = new() { ByUser = user, Completed = true, Obsolete = false };

        PaginatedList<LeaderboardEntry> entries =
            database.GetPaginatedLeaderboardEntries(level, order, descending, filters, 0, 1, user);
        return entries.Items.Select(e => new LeaderboardEntryResponse(e, order, filters)).ToArray();
    }
}