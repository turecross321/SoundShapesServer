namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiReportResponse
{
    public string Id { get; set; }
    public string ContentId { get; set; }
    public int ContentType { get; set; }
    public int ReportReasonId { get; set; }
    public DateTimeOffset Issued { get; set; }
    public string IssuerId { get; set; }
}