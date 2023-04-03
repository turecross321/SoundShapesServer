using Realms;

namespace SoundShapesServer.Types.Relations;

public class FollowRelation : RealmObject
{
    public GameUser follower { get; set; }
    public GameUser userBeingFollowed { get; set; }
}