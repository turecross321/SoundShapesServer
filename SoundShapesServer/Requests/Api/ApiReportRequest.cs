using SoundShapesServer.Types.Reports;
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618

namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiReportRequest
{
    public ReportContentType ContentType { get; set; }
    public string ContentId { get; set; }
    public ReportReasonType ReasonType { get; set; }
}