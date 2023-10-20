using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Game;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.LeaderboardHelper;

namespace SoundShapesServer.Endpoints.Game;

public class LeaderboardEndpoints : EndpointGroup
{
    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.post", HttpMethods.Post)]
    [GameEndpoint("~identity:{userId}/~record:%2F~level%3A{arguments}", HttpMethods.Post)]
    public Response SubmitScore(RequestContext context, GameDatabaseContext database, GameUser user, GameToken token, string userId, string? arguments, string body, string levelId)
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
        
        if (!LevelHelper.IsUserAllowedToAccessLevel(level, user))
            return HttpStatusCode.NotFound;

        LeaderboardSubmissionRequest deSerializedRequest = DeSerializeSubmission(body);
        
        
        if (deSerializedRequest.Completed) database.AddCompletionToLevel(user, level);
        database.CreatePlay(user, level);
        database.AddDeathsToLevel(user, level, deSerializedRequest.Deaths);
        database.SetLevelDifficulty(level);

        database.CreateLeaderboardEntry(deSerializedRequest, user, level, token.PlatformType);

        return HttpStatusCode.OK;
    }

    [GameEndpoint("global/~campaign:{levelId}/~leaderboard.page")] // campaign levels
    [GameEndpoint("~level:{levelId}/~leaderboard.page")] // community levels
    [GameEndpoint("{levelId}/~leaderboard.page")] // recent activity community levels 
    public ListResponse<LeaderboardEntryResponse>? GetLeaderboard(RequestContext context, GameDatabaseContext database, GameUser user, string levelId)
    {
        (int from, int count, bool _) = context.GetPageData();
        const bool descending = false;

        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return null;

        LeaderboardFilters filters = new (level, obsolete: false, completed: true);
        (LeaderboardEntry[] paginatedEntries, int totalEntries) = database.GetPaginatedLeaderboardEntries(LeaderboardOrderType.Score, descending, filters, from, count, user);

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
        
        LeaderboardFilters filters = new (level, user, completed:true, obsolete:false);

        (LeaderboardEntry[] paginatedEntries, int _) = database.GetPaginatedLeaderboardEntries(LeaderboardOrderType.Score, false, filters, 0, 1, user);
        
        return paginatedEntries.Select(e=> new LeaderboardEntryResponse(e, e.Position() + 1)).ToArray();
    }
}