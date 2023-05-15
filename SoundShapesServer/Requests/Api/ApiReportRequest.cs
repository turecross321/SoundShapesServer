namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiReportRequest
{
    public ApiReportRequest(int contentType, string contentId, int reasonType)
    {
        ContentType = contentType;
        ContentId = contentId;
        ReasonType = reasonType;
    }
    
    public int ContentType { get; set; }
    public string ContentId { get; set; }
    public int ReasonType { get; }
}