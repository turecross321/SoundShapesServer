namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateAlbumRequest
{
    public ApiCreateAlbumRequest(string name, string author, string linerNotes, string[] levelIds)
    {
        Name = name;
        Author = author;
        LinerNotes = linerNotes;
        LevelIds = levelIds;
    }

    public string Name { get; set; }
    public string Author { get; set; }
    public string LinerNotes { get; set; }
    public string[] LevelIds { get; set; }
}