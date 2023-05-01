using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Responses.Api.Albums;

public class ApiAlbumResponse
{
    public ApiAlbumResponse(GameAlbum album)
    {
        Id = album.Id;
        Artist = album.Artist;
        Name = album.Name;
        LinerNotes = album.LinerNotes;
        TotalLevels = album.Levels.Count;
        CreationDate = album.CreationDate;
        ModificationDate = album.ModificationDate;
    }

    public string Id { get; set; }
    public string Artist { get; set; }
    public string Name { get; set; }
    public string LinerNotes { get; set; }
    public int TotalLevels { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}