using HtmlAgilityPack;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static LinerNoteResponse[] HtmlToLinerNotes(string html)
    {
        HtmlDocument document = new ();
        document.LoadHtml(html);

        List<LinerNoteResponse> linerNotes = new ();

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

    public static IQueryable<GameAlbum> OrderAlbums(IQueryable<GameAlbum> albums, AlbumOrderType orderType, bool descending)
    {
        IQueryable<GameAlbum> response = albums;

        response = orderType switch
        {
            AlbumOrderType.CreationDate => response.OrderBy(a=>a.CreationDate).AsQueryable(),
            AlbumOrderType.ModificationDate => response.OrderBy(a=>a.ModificationDate).AsQueryable(),
            AlbumOrderType.Plays => response.OrderBy(a=> a.Levels.Select(l=>l.PlaysCount)).AsQueryable(),
            AlbumOrderType.UniquePlays => response.OrderBy(a=> a.Levels.Select(l=> l.UniquePlaysCount)).AsQueryable(),
            AlbumOrderType.LevelsCount => response.OrderBy(a=>a.Levels.Count).AsQueryable(),
            AlbumOrderType.FileSize => response.OrderBy(a=> a.Levels.Select(l=>l.FileSize).Sum()).AsQueryable(),
            AlbumOrderType.Difficulty => response.OrderBy(a=> a.Levels.Select(l=>l.Difficulty).Sum()).AsQueryable(),
            _ => OrderAlbums(response, AlbumOrderType.CreationDate, descending)
        };

        if (descending) response = response.AsEnumerable().Reverse().AsQueryable();

        return response;
    }
}