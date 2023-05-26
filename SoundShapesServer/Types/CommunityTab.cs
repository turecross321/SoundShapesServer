using Realms;
using SoundShapesServer.Types.Users;

#pragma warning disable CS8618

namespace SoundShapesServer.Types;

public class CommunityTab : RealmObject
{
    [PrimaryKey] public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonLabel { get; set; }
    public string Query { get; set; }
    public string? ThumbnailFilePath { get; set; }
    public GameUser Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}