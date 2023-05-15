using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Game.Albums;

public class AlbumMetadata
{
    public AlbumMetadata(GameAlbum album)
    {
        LinerNotesWrapper linerNoteWrapper = new (AlbumHelper.HtmlToLinerNotes(album.LinerNotes));
        string linerNotesString = JsonConvert.SerializeObject(linerNoteWrapper);
        
        Author = album.Author;
        LinerNotes = linerNotesString;
        SidePanelUrl = ResourceHelper.GenerateAlbumResourceUrl(album.Id, AlbumResourceType.SidePanel);
        CreationDate = album.CreationDate.ToString();
        Name = album.Name;
        ThumbnailUrl = ResourceHelper.GenerateAlbumResourceUrl(album.Id, AlbumResourceType.Thumbnail);
    }

    [JsonProperty("albumArtist")] public string Author { get; set; }
    [JsonProperty("linerNotes")] public string LinerNotes { get; set; }
    [JsonProperty("sidePanelURL")] public string SidePanelUrl { get; set; }
    [JsonProperty("date")] public string CreationDate { get; set; }
    [JsonProperty("displayName")] public string Name { get; set; }
    [JsonProperty("thumbnailURL")] public string ThumbnailUrl { get; set; }
}