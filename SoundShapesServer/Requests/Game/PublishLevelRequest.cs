using SoundShapesServer.Requests.Api;

namespace SoundShapesServer.Requests.Game;

// ReSharper disable once ClassNeverInstantiated.Global
public class PublishLevelRequest
{
    public PublishLevelRequest(string title, int language, string id, long fileSize, DateTimeOffset? modified = null)
    {
        Title = title;
        Language = language;
        Id = id;
        FileSize = fileSize;
        Modified = modified ?? DateTimeOffset.UtcNow;
    }

    public PublishLevelRequest(ApiPublishLevelRequest request, string levelId)
    {
        Title = request.Title;
        Language = request.Language;
        Id = levelId;
        FileSize = 0;
        Modified = request.Modified ?? DateTimeOffset.UtcNow;
    }

    public string Title { get; }
    public int Language { get; }
    public string Id { get; }
    public long FileSize { get; }
    public DateTimeOffset Modified { get; set; }    
}