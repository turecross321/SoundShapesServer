using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints.Game;

public class ReportEndpoints : EndpointGroup
{
    [GameEndpoint("~grief:*.post", Method.Post)]
    public Response ReportLevel(RequestContext context, RealmDatabaseContext database, GameUser user, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        string reportReasonIdString = parser.GetParameterValue("reportReasonId");
        if (int.TryParse(reportReasonIdString, out int reportReasonId) == false) return HttpStatusCode.BadRequest;
        
        string formattedLevelId = parser.GetParameterValue("item");
        string levelId = IdFormatter.DeFormatLevelIdAndVersion(formattedLevelId);
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author?.Id == user.Id) return HttpStatusCode.BadRequest;
        
        database.SubmitReport(user, level.Id, ServerContentType.Level, reportReasonId);

        return HttpStatusCode.OK;
    }
}