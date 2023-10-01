using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Reports;

public class ApiReportEndpoint : EndpointGroup
{
    [ApiEndpoint("reports/create", HttpMethods.Post)]
    [DocSummary("Creates report.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.UserNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LevelNotFoundWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.LeaderboardEntryNotFoundWhen)]
    public ApiOkResponse CreateReport(RequestContext context, GameDatabaseContext database, GameUser user, ApiReportRequest body)
    {
        GameUser? userBeingReported = null;
        GameLevel? level = null;
        LeaderboardEntry? leaderboardEntry = null;
        
        switch (body.ContentType)
        {
            case ReportContentType.User:
            {
                userBeingReported = database.GetUserWithId(body.ContentId);
                if (userBeingReported == null) 
                    return ApiNotFoundError.UserNotFound;
                break;
            }
            case ReportContentType.Level:
            {
                level = database.GetLevelWithId(body.ContentId);
                if (level == null)
                    return ApiNotFoundError.LevelNotFound;
                break;
            }
            case ReportContentType.LeaderboardEntry:
            {
                leaderboardEntry = database.GetLeaderboardEntryWithId(body.ContentId);
                if (leaderboardEntry == null)
                    return ApiNotFoundError.LeaderboardEntryNotFound;
                break;
            }
            default:
                return ApiBadRequestError.InvalidContentType;
        }

        database.CreateReport(user, body.ContentType, body.ReasonType, userBeingReported, level, leaderboardEntry);
        return new ApiOkResponse();
    }
}