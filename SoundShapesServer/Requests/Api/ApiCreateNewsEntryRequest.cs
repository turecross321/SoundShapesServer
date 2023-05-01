namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateNewsEntryRequest
{
    public ApiCreateNewsEntryRequest(string language, string title, string summary, string fullText, string url)
    {
        Language = language;
        Title = title;
        Summary = summary;
        FullText = fullText;
        Url = url;
    }

    public string Language { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}