using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class ReportEndpoints : EndpointGroup
{
    [GameEndpoint("~grief:*.post", Method.Post)]
    public Response CreateReportForLevel(RequestContext context, GameDatabaseContext database, GameUser user, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        string reportReasonIdString = parser.GetParameterValue("reportReasonId");
        ReportReasonType reportReasonType = Enum.Parse<ReportReasonType>(reportReasonIdString);

        string formattedLevelId = parser.GetParameterValue("item");
        string levelId = IdFormatter.DeFormatLevelIdAndVersion(formattedLevelId);
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null) return HttpStatusCode.NotFound;

        if (level.Author.Id == user.Id) return HttpStatusCode.BadRequest;
        
        database.CreateReport(user, ReportContentType.Level, reportReasonType, contentLevel:level);

        return HttpStatusCode.OK;
    }
}