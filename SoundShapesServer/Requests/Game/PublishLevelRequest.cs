using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Requests.Game;

// ReSharper disable once ClassNeverInstantiated.Global
public class PublishLevelRequest
{
    public PublishLevelRequest(string name, int language, DateTimeOffset? created = null, LevelVisibility visibility = LevelVisibility.Public)
    {
        Name = name;
        Language = language;
        CreationDate = created ?? DateTimeOffset.UtcNow;
        Visibility = visibility;
    }

    public PublishLevelRequest(ApiPublishLevelRequest request)
    {
        Name = request.Name;
        Language = request.Language;
        FileSize = 0;
        if (request.CreationDate != null)
            CreationDate = DateTimeOffset.FromUnixTimeSeconds((long)request.CreationDate);
    }

    public PublishLevelRequest(ApiEditLevelRequest request)
    {
        Name = request.Name;
    }

    public string Name { get; set; }
    public int Language { get; set; }
    public long FileSize { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public LevelVisibility Visibility { get; set; }
}