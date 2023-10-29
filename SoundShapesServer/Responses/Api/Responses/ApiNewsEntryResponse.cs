using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiNewsEntryResponse : IApiResponse
{
    public ApiNewsEntryResponse(NewsEntry entry)
    {
        Id = entry.Id.ToString()!;
        CreationDate = entry.CreationDate;
        ModificationDate = entry.ModificationDate;
        Language = entry.Language;
        Author = new ApiUserBriefResponse(entry.Author);
        Title = entry.Title;
        Summary = entry.Summary;
        FullText = entry.FullText;
        Url = entry.Url;
    }
    
    public string Id { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public string Language { get; set; }
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}