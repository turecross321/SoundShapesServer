using System.Net;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Http;
using HttpMultipartParser;
using SoundShapesServer.Database;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Game;

public class ReportEndpoints : EndpointGroup
{
    [GameEndpoint("~grief:*.post", HttpMethods.Post)]
    public Response CreateReportForLevel(RequestContext context, GameDatabaseContext database, GameUser user, Stream body)
    {
        MultipartFormDataParser? parser = MultipartFormDataParser.Parse(body);

        string reportReasonIdString = parser.GetParameterValue("reportReasonId");
        ReportReasonType reportReasonType = Enum.Parse<ReportReasonType>(reportReasonIdString);

        string formattedLevelId = parser.GetParameterValue("item");
        string levelId = IdHelper.DeFormatLevelIdAndVersion(formattedLevelId);
        GameLevel? level = database.GetLevelWithId(levelId);
        if (level == null || !level.HasUserAccess(user)) 
            return HttpStatusCode.NotFound;

        if (level.Author.Id == user.Id) return HttpStatusCode.BadRequest;
        
        database.CreateReport(user, ReportContentType.Level, reportReasonType, contentLevel:level);

        return HttpStatusCode.OK;
    }
}