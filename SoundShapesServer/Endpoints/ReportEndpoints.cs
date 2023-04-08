using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Endpoints;

public class ReportEndpoints : EndpointGroup
{
    [Endpoint("/otg/~grief:*.post", Method.Post)]
    public Response ReportLevel(RequestContext context, RealmDatabaseContext database, GameUser user, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        string reportReasonIdString = parser.GetParameterValue("reportReasonId");
        if (int.TryParse(reportReasonIdString, out int reportReasonId) == false) return HttpStatusCode.BadRequest;
        
        string formattedLevelId = parser.GetParameterValue("item");
        string levelId = IdFormatter.DeFormatLevelIdAndVersion(formattedLevelId);
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;
        
        database.SubmitReport(user, level, reportReasonId);
        
        return HttpStatusCode.OK;
    }
}