using MongoDB.Bson;
using Realms;
using SoundShapesServer.Database;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Events;

public class GameEvent : RealmObject
{
    public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    
    // Realm can't store enums, use recommended workaround
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with EventType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _EventType { get; set; }
    public EventType EventType
    {
        get => (EventType)_EventType;
        set => _EventType = (int)value;
    }

    public GameUser Actor { get; init; } = null!;
    public string DataId { get; set; }
    internal int _DataType { get; set; }

    public EventDataType DataType
    {
        get => (EventDataType)_DataType;
        set => _DataType = (int)value;
    }
    
    public object Data(GameDatabaseContext database)
    {
        return DataType switch
        {
            EventDataType.Level => database.GetLevelWithId(DataId),
            EventDataType.LeaderboardEntry => database.GetLeaderboardEntry(DataId)!,
            EventDataType.User => database.GetUserWithId(DataId)!,
            EventDataType.Album => database.GetAlbumWithId(DataId)!,
            EventDataType.NewsEntry => database.GetNewsEntryWithId(DataId)!
        };
    }
    
    public DateTimeOffset CreationDate { get; set; }
    // ReSharper disable once InconsistentNaming (can't fix due to conflict with PlatformType)
    // ReSharper disable once MemberCanBePrivate.Global
    internal int _PlatformType { get; init; } = (int)PlatformType.Unknown;
    public PlatformType PlatformType
    {
        get => (PlatformType)_PlatformType;
        init => _PlatformType = (int)value;
    }
}