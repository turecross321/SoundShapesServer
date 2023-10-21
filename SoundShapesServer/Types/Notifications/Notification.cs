using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Notifications;

public class Notification : RealmObject
{
    public ObjectId Id { get; init; } = ObjectId.GenerateNewId();
    public GameUser User { get; init; }
    internal int _NotificationType { get; set; }
    public NotificationType NotificationType
    {
        get => (NotificationType)_NotificationType;
        set => _NotificationType = (int)value;
    }
    public GameEvent Event { get; init; }
    public bool HasBeenRead { get; init; }
}