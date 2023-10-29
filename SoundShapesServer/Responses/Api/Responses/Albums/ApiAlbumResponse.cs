using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Albums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Albums;

public class ApiAlbumResponse : IApiResponse
{
    public ApiAlbumResponse(GameAlbum album)
    {
        Id = album.Id.ToString()!;
        Author = new ApiUserBriefResponse(album.Author);
        Name = album.Name;
        LinerNotes = album.LinerNotes;
        TotalLevels = album.Levels.Count;
        CreationDate = album.CreationDate;
        ModificationDate = album.ModificationDate;
    }

    public string Id { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public string Name { get; set; }
    public string LinerNotes { get; set; }
    public int TotalLevels { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}