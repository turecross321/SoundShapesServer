using Realms;
using SoundShapesServer.Types.Users;

#pragma warning disable CS8618

namespace SoundShapesServer.Types;

public class CommunityTab : RealmObject
{
    [PrimaryKey] public string Id { get; init; }
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with TokenType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _ContentType { get; set; }

    public GameContentType ContentType
    {
        get => (GameContentType)_ContentType;
        set => _ContentType = (int)value;
    }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonLabel { get; set; }
    public string Query { get; set; }
    public string? ThumbnailFilePath { get; set; }
    public GameUser Author { get; set; }
    public DateTimeOffset CreationDate { get; init; }
    public DateTimeOffset ModificationDate { get; set; }
}