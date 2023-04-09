using Realms;

namespace SoundShapesServer.Types;

public class NewsEntry : RealmObject
{
    public string Language { get; set; } = "global";
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
}