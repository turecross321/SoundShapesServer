namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateAlbumRequest
{
    public ApiCreateAlbumRequest(string name, string artist, string linerNotes, string[] levelIds)
    {
        Name = name;
        Artist = artist;
        LinerNotes = linerNotes;
        LevelIds = levelIds;
    }

    public string Name { get; set; }
    public string Artist { get; set; }
    public string LinerNotes { get; set; }
    public string[] LevelIds { get; set; }
}