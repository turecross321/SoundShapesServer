using SoundShapesServer.Requests.Api;

namespace SoundShapesServer.Requests.Game;

// ReSharper disable once ClassNeverInstantiated.Global
public class PublishLevelRequest
{
    public PublishLevelRequest(string name, int language, DateTimeOffset? created = null)
    {
        Name = name;
        Language = language;
        Created = created ?? DateTimeOffset.UtcNow;
    }

    public PublishLevelRequest(ApiPublishLevelRequest request)
    {
        Name = request.Name;
        Language = request.Language;
        FileSize = 0;
        if (request.CreationDate != null)
            Created = DateTimeOffset.FromUnixTimeSeconds((long)request.CreationDate);
    }

    public PublishLevelRequest(ApiEditLevelRequest request)
    {
        Name = request.Name;
    }

    public string Name { get; set; }
    public int Language { get; }
    public long FileSize { get; }
    public DateTimeOffset Created { get; }    
}