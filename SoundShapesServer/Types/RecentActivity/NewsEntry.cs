using Realms;

namespace SoundShapesServer.Types.RecentActivity;

public class NewsEntry : RealmObject
{
    public NewsEntry(GameUser author)
    {
        Author = author;
    }
    
    // Realm cries of this doesn't exist
    #pragma warning disable CS8618
    public NewsEntry() {}
    #pragma warning restore CS8618

    [PrimaryKey] [Required] public string Id { get; set; } = "";
    public DateTimeOffset Date { get; set; }
    public GameUser Author { get; set; }
    public string Language { get; set; } = "global";
    public string Title { get; set; }  = "News";
    public string Summary { get; set; } = "There are no news yet!";
    public string FullText { get; set; } = "";
    public string Url { get; set; } = "";
}