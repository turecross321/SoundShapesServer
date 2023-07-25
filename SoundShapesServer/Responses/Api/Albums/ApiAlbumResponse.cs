using SoundShapesServer.Types.Albums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Albums;

public class ApiAlbumResponse : IApiResponse
{
    public ApiAlbumResponse(GameAlbum album)
    {
        Id = album.Id;
        Author = album.Author;
        Name = album.Name;
        LinerNotes = album.LinerNotes;
        TotalLevels = album.Levels.Count;
        CreationDate = album.CreationDate.ToUnixTimeSeconds();
        ModificationDate = album.ModificationDate.ToUnixTimeSeconds();
    }

    public string Id { get; set; }
    public string Author { get; set; }
    public string Name { get; set; }
    public string LinerNotes { get; set; }
    public int TotalLevels { get; set; }
    public long CreationDate { get; set; }
    public long ModificationDate { get; set; }
}