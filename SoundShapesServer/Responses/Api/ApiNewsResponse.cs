using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api;

public class ApiNewsResponse
{
    public ApiNewsResponse(NewsEntry entry)
    {
        Id = entry.Id;
        Date = entry.Date;
        Language = entry.Language;
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = entry.Url;
    }
    
    public string Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public string Language { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}