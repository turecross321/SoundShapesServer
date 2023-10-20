using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Relations;

public class UserFollowRelation : RealmObject
{
    public DateTimeOffset Date { get; init; }
    public GameUser Follower { get; init; } = null!;
    public GameUser Recipient { get; init; } = null!;
}