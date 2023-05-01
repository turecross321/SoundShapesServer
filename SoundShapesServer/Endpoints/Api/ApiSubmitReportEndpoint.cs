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
    [ApiEndpoint("reports/submit/{id}", Method.Post)]
    public Response ReportContent(RequestContext context, RealmDatabaseContext database, GameUser user, string id, ApiReportRequest body)
    {
        GameLevel? level = database.GetLevelWithId(id);
        GameUser? userToReport = database.GetUserWithId(id);

        if (level == null && userToReport == null) return HttpStatusCode.NotFound;

        ServerContentType type = level != null ? ServerContentType.Level : ServerContentType.User;

        database.SubmitReport(user, id, type, body.ReportReasonId);
        return HttpStatusCode.Created;
    }
}