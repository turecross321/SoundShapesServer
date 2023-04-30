namespace SoundShapesServer.Types;

public class CommunityTab
{
    public CommunityTab(string buttonLabel, string query, string panelDescription, string imageUrl, string panelTitle)
    {
        ButtonLabel = buttonLabel;
        Query = query;
        PanelDescription = panelDescription;
        ImageUrl = imageUrl;
        PanelTitle = panelTitle;
    }

    public string ButtonLabel { get; init; }
    public string Query { get; init; }
    public string PanelDescription { get; init; }
    public string ImageUrl { get; init; }
    public string PanelTitle { get; init; }
}