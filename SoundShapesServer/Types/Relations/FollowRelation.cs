using Realms;

namespace SoundShapesServer.Types.Relations;

public class FollowRelation : RealmObject
{
    public GameUser Follower { get; set; }
    public GameUser Recipient { get; set; }
}