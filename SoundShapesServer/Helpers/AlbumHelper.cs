using HtmlAgilityPack;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static LinerNoteResponse[] HtmlToLinerNotes(string html)
    {
        HtmlDocument document = new();
        document.LoadHtml(html);

        List<LinerNoteResponse> linerNotes = new();

        IEnumerable<HtmlNode> openingNodes = document.DocumentNode.DescendantsAndSelf().Where(n => n.NodeType == HtmlNodeType.Element);

        foreach (HtmlNode node in openingNodes)
        {
            if (node.ChildNodes.Any(n => n.NodeType != HtmlNodeType.Text)) continue;

            FontType fontType = node.Name switch
            {
                "h1" => FontType.Title,
                "h2" => FontType.Heading,
                _ => FontType.Normal
            };
                
            linerNotes.Add(new LinerNoteResponse(fontType, node.InnerText));
        }

        return linerNotes.ToArray();
    }
}