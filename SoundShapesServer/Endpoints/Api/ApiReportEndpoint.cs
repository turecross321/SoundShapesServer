using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api;

public class ApiReportEndpoint : EndpointGroup
{
    [ApiEndpoint("reports/create", Method.Post)]
    public Response CreateReport(RequestContext context, GameDatabaseContext database, GameUser user, ApiReportRequest body)
    {
        ReportContentType contentType = Enum.Parse<ReportContentType>(body.ContentType.ToString());
        
        if (body.ReasonType > Enum.GetNames(typeof(ReportReasonType)).Length || body.ReasonType < 0) return HttpStatusCode.BadRequest; // WHY DOES THE PARSER NOT DO THIS
        ReportReasonType reportReasonType = Enum.Parse<ReportReasonType>(body.ReasonType.ToString());
        
        GameUser? userBeingReported = null;
        GameLevel? level = null;
        LeaderboardEntry? leaderboardEntry = null;
        
        switch (contentType)
        {
            case ReportContentType.Level:
            {
                level = database.GetLevelWithId(body.ContentId);
                if (level == null) return HttpStatusCode.NotFound;
                break;
            }
            case ReportContentType.User:
            {
                userBeingReported = database.GetUserWithId(body.ContentId);
                if (userBeingReported == null) return HttpStatusCode.NotFound;
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

        database.CreateReport(user, contentType, reportReasonType, userBeingReported, level, leaderboardEntry);
        return HttpStatusCode.Created;
    }
}