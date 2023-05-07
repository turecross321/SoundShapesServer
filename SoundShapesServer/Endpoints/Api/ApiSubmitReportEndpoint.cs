using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Api;

public class ApiSubmitReportEndpoint : EndpointGroup
{
    [ApiEndpoint("reports/create", Method.Post)]
    public Response ReportContent(RequestContext context, GameDatabaseContext database, GameUser user, ApiReportRequest body)
    {
        GameLevel? level = database.GetLevelWithId(body.ContentId);
        GameUser? userToReport = database.GetUserWithId(body.ContentId);

        if (level == null && userToReport == null) return HttpStatusCode.NotFound;

        string type = level != null ? ServerContentType.Level : ServerContentType.User;

        database.SubmitReport(user, body.ContentId, type, body.ReportReasonId);
        return HttpStatusCode.Created;
    }
}