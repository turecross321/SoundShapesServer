using Realms;

namespace SoundShapesServer.Types.Relations;

public class FollowRelation : RealmObject
{
    public DateTimeOffset Date { get; init; }
    public GameUser Follower { get; init; } = new();
    public GameUser Recipient { get; init; } = new();
}