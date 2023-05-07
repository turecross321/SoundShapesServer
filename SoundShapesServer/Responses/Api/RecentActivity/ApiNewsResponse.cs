using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Responses.Api.RecentActivity;

public class ApiNewsResponse
{
    public ApiNewsResponse(NewsEntry entry)
    {
        Id = entry.Id;
        CreationDate = entry.CreationDate;
        ModificationDate = entry.ModificationDate;
        Language = entry.Language;
        AuthorId = entry.Author.Id;
        AuthorUsername = entry.Author.Username;
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = entry.Url;
    }
    
    public string Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public string AuthorUsername { get; set; }
    public string AuthorId { get; set; }
    public string Language { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}