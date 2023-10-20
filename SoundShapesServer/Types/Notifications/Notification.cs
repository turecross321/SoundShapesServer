using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Notifications;

public class Notification : RealmObject
{
    public ObjectId Id { get; init; }= ObjectId.GenerateNewId();
    public GameUser User { get; set; }
    public GameEvent Event { get; set; }
    public bool HasBeenRead { get; set; }
}