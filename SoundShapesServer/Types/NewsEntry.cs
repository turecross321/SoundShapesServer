using Realms;

namespace SoundShapesServer.Types;

public class NewsEntry : RealmObject
{
    public string Language { get; init; } = "global";
    public string Title { get; init; }  = "News";
    public string Summary { get; init; } = "There are no news yet!";
    public string FullText { get; init; } = "";
    public string Url { get; init; } = "";
}