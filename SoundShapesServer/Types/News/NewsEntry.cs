using Realms;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.News;

public class NewsEntry : RealmObject
{
    public NewsEntry(GameUser author, ApiCreateNewsEntryRequest request)
    {
        Id = Guid.NewGuid().ToString();
        CreationDate = DateTimeOffset.UtcNow;
        ModificationDate = DateTimeOffset.UtcNow;
        Author = author;
        Language = request.Language ?? "global";
        Title = request.Title ?? "";
        Summary = request.Summary ?? "";
        FullText = request.FullText ?? "";
        Url = string.IsNullOrEmpty(request.Url) ? "0.0.0.0" : request.Url; // An url crashes the Vita version
        CharacterCount = FullText.Length;
    }
    
    // Realm cries of this doesn't exist
    #pragma warning disable CS8618
    public NewsEntry()
    {
        Title = "News";
        Summary = "There are no news yet.";
        FullText = "There are no news yet.";
        Url = "0.0.0.0";
    }
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; init; } = "";
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ModificationDate { get; set; }
    public GameUser Author { get; init; }
    public string Language { get; set; } = "global";
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
    public int CharacterCount { get; set; }
}