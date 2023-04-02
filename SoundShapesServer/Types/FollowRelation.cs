using Realms;

namespace SoundShapesServer.Types;

public class FollowRelation : RealmObject
{
    public GameUser follower { get; set; }
    public GameUser userBeingFollowed { get; set; }
}