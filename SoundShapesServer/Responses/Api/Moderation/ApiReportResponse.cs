using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportResponse
{
    public ApiReportResponse(Report report)
    {
        Id = report.Id;
        ContentId = report.ContentId;
        ContentType = report.ContentType;
        ReportReasonId = report.ReportReasonId;
        Issued = report.Issued;
        IssuerId = report.Issuer.Id;
    }

    public string Id { get; set; }
    public string ContentId { get; set; }
    public int ContentType { get; set; }
    public int ReportReasonId { get; set; }
    public DateTimeOffset Issued { get; set; }
    public string IssuerId { get; set; }
}