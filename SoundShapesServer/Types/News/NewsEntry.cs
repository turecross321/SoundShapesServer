using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.News;

public class NewsEntry : RealmObject
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId(); 
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ModificationDate { get; set; }
    public GameUser Author { get; set; }
    public string Language { get; set; } = "global";
    public string Title { get; set; }
    public string Summary { get; set; }
    public string FullText { get; set; }
    public string Url { get; set; }
    public int CharacterCount { get; set; }
    public string? ThumbnailFilePath { get; set; }
}