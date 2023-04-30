using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiNewsResponse
{
    public ApiNewsResponse(NewsEntry entry)
    {
        Language = entry.Language;
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = entry.Url;
    }

    public string Language { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}