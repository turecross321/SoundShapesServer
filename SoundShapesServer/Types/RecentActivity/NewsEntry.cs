using Realms;
using SoundShapesServer.Requests.Api;

namespace SoundShapesServer.Types.RecentActivity;

public class NewsEntry : RealmObject
{
    public NewsEntry(GameUser author, ApiCreateNewsEntryRequest request)
    {
        Id = Guid.NewGuid().ToString();
        CreationDate = DateTimeOffset.UtcNow;
        ModificationDate = DateTimeOffset.UtcNow;
        Author = author;
        Language = request.Language;
        Title = request.Language;
        Summary = request.Language;
        FullText = request.Language;
        Url = request.Url;
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
}