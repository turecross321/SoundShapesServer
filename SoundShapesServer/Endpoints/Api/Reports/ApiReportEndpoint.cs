using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Moderation;

public class ApiReportEndpoint : EndpointGroup
{
    [ApiEndpoint("reports/create", Method.Post)]
    public Response CreateReport(RequestContext context, GameDatabaseContext database, GameUser user, ApiReportRequest body)
    {
        GameUser? userBeingReported = null;
        GameLevel? level = null;
        LeaderboardEntry? leaderboardEntry = null;
        
        switch (body.ContentType)
        {
            case ReportContentType.User:
            {
                userBeingReported = database.GetUserWithId(body.ContentId);
                if (userBeingReported == null) return HttpStatusCode.NotFound;
                break;
            }
            case ReportContentType.Level:
            {
                level = database.GetLevelWithId(body.ContentId);
                if (level == null) return HttpStatusCode.NotFound;
                break;
            }
            case ReportContentType.LeaderboardEntry:
            {
                leaderboardEntry = database.GetLeaderboardEntryWithId(body.ContentId);
                if (leaderboardEntry == null) return HttpStatusCode.NotFound;
                break;
            }
            default:
                return HttpStatusCode.BadRequest;
        }

        database.CreateReport(user, body.ContentType, body.ReasonType, userBeingReported, level, leaderboardEntry);
        return HttpStatusCode.Created;
    }
}