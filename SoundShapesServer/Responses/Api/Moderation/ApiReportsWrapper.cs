using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportsWrapper
{
    public ApiReportResponse[] Reports { get; set; }
    public int Count { get; set; }
}