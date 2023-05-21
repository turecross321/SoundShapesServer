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
        Created = request.ModificationDate ?? DateTimeOffset.UtcNow;
    }

    public PublishLevelRequest(ApiEditLevelRequest request)
    {
        Name = request.Name;
        Language = request.Language;
    }

    public string Name { get; }
    public int Language { get; }
    public long FileSize { get; }
    public DateTimeOffset Created { get; }    
}