using Realms;

namespace SoundShapesServer.Types.Relations;

public class FollowRelation : RealmObject
{
    public GameUser? Follower { get; init; }
    public GameUser? Recipient { get; init; }
}