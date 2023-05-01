namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiReportRequest
{
    public ApiReportRequest(string contentId, int reportReasonId)
    {
        ContentId = contentId;
        ReportReasonId = reportReasonId;
    }
    
    public string ContentId { get; set; }
    public int ReportReasonId { get; }
}