using Realms;

namespace SoundShapesServer.Types;

public class NewsEntry : RealmObject
{
    // TODO: TODO: TODO: ADD ID
    public string Language { get; set; } = "global";
    public string Title { get; set; }  = "News";
    public string Summary { get; set; } = "There are no news yet!";
    public string FullText { get; set; } = "";
    public string Url { get; set; } = "";
}