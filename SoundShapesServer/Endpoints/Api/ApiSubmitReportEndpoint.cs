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
    [ApiEndpoint("user/{id}/report", Method.Post)]
    public Response ReportUser(RequestContext context, RealmDatabaseContext database, GameUser user, string id, ApiReportRequest body)
    {
        GameUser? userBeingReported = database.GetUserWithId(id);
        if (userBeingReported == null) return HttpStatusCode.NotFound;
        
        if (userBeingReported.Id == user.Id) return HttpStatusCode.BadRequest;
        
        database.SubmitReport(user, userBeingReported.Id, ServerContentType.User, body.ReportReasonId);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("level/{id}/report", Method.Post)]
    public Response ReportLevel(RequestContext context, RealmDatabaseContext database, GameUser user, string id, ApiReportRequest body)
    {
        GameLevel? level = database.GetLevelWithId(id);
        if (level == null) return HttpStatusCode.NotFound;
        
        if (level.Author.Id == user.Id) return HttpStatusCode.BadRequest;
        
        database.SubmitReport(user, level.Id, ServerContentType.Level, body.ReportReasonId);
        return HttpStatusCode.Created;
    }
}