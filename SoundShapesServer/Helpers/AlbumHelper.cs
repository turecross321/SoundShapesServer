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

    public static IQueryable<GameAlbum> OrderAlbums(IEnumerable<GameAlbum> albums, AlbumOrderType orderType)
    {
        return orderType switch
        {
            AlbumOrderType.CreationDate => albums.OrderBy(a=>a.CreationDate).AsQueryable(),
            AlbumOrderType.ModificationDate => albums.OrderBy(a=>a.ModificationDate).AsQueryable(),
            AlbumOrderType.Plays => albums.OrderBy(a=> a.Levels.Select(l=>l.Plays).Sum()).AsQueryable(),
            AlbumOrderType.UniquePlays => albums.OrderBy(a=> a.Levels.Select(l=> l.UniquePlays.Count).Sum()).AsQueryable(),
            AlbumOrderType.LevelCount => albums.OrderBy(a=>a.Levels.Count).AsQueryable(),
            AlbumOrderType.FileSize => albums.OrderBy(a=> a.Levels.Select(l=>l.FileSize).Sum()).AsQueryable(),
            AlbumOrderType.Difficulty => albums.OrderBy(a=> a.Levels.Select(l=>l.Difficulty).Sum()).AsQueryable(),
            _ => OrderAlbums(albums, AlbumOrderType.CreationDate)
        };
    }
}