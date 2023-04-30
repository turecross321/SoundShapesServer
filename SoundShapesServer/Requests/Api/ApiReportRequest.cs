namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiReportRequest
{
    public ApiReportRequest(int reportReasonId)
    {
        ReportReasonId = reportReasonId;
    }
    
    public int ReportReasonId { get; }
}