namespace SoundShapesServer.Requests.Api;

public class ApiNewsEntryRequest
{
    public string Language { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Summary { get; set; } = null!;
    public string FullText { get; set; } = null!;
    public string Url { get; set; } = null!;
}