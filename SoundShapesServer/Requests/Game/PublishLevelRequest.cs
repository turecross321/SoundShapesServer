using SoundShapesServer.Requests.Api;

namespace SoundShapesServer.Requests.Game;

// ReSharper disable once ClassNeverInstantiated.Global
public class PublishLevelRequest
{
    public PublishLevelRequest(string name, int language, string id, long fileSize, DateTimeOffset? modified = null)
    {
        Name = name;
        Language = language;
        Id = id;
        FileSize = fileSize;
        Modified = modified ?? DateTimeOffset.UtcNow;
    }

    public PublishLevelRequest(ApiPublishLevelRequest request, string levelId)
    {
        Name = request.Name;
        Language = request.Language;
        Id = levelId;
        FileSize = 0;
        Modified = request.Modified ?? DateTimeOffset.UtcNow;
    }

    public PublishLevelRequest(ApiEditLevelRequest request)
    {
        Name = request.Name;
        Language = request.Language;
    }

    public string Name { get; }
    public int Language { get; }
    public string Id { get; } = "";
    public long FileSize { get; }
    public DateTimeOffset Modified { get; set; }    
}