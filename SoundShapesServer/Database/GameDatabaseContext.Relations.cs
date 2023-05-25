using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public bool FollowUser(GameUser follower, GameUser recipient)
    {
        if (IsUserFollowingOtherUser(follower, recipient)) return false;

        FollowRelation relation = new()
        {
            Date = DateTimeOffset.UtcNow,
            Follower = follower,
            Recipient = recipient
        };
        _realm.Write(() =>
        {
            _realm.Add(relation);
            follower.FollowingCount = follower.Following.Count();
            recipient.FollowersCount = recipient.Followers.Count();
        });

        CreateEvent(follower, EventType.Follow, recipient);
        
        return true;
    }
    
    public bool UnFollowUser(GameUser follower, GameUser recipient)
    {
        if (!IsUserFollowingOtherUser(follower, recipient)) return false;
        
        FollowRelation? relation = _realm.All<FollowRelation>().FirstOrDefault(f => f.Follower == follower && f.Recipient == recipient);

        if (relation == null) return false;
        
        _realm.Write(() =>
        {
            _realm.Remove(relation);
        });
        
        return true;
    }

    public bool LikeLevel(GameUser liker, GameLevel level)
    {
        if (IsUserLikingLevel(liker, level)) return false;
        LevelLikeRelation relation = new(DateTimeOffset.UtcNow, liker, level);
        
        _realm.Write(() =>
        {
            _realm.Add(relation);
            liker.LikedLevelsCount = liker.LikedLevels.Count();
        });
        
        CreateEvent(liker, EventType.Like, null, level);

        return true;
    }
    
    public bool UnLikeLevel(GameUser liker, GameLevel level)
    {
        if (!IsUserLikingLevel(liker, level)) return false;

        LevelLikeRelation? relation = _realm.All<LevelLikeRelation>().FirstOrDefault(l => l.Liker == liker && l.Level == level);

        if (relation == null) return false;
        
        _realm.Write(() =>
        {
            _realm.Remove(relation);
        });
        
        return true;
    }

    public bool IsUserLikingLevel(GameUser liker, GameLevel level)
    {
        LevelLikeRelation? relation = _realm.All<LevelLikeRelation>().FirstOrDefault(l => l.Liker == liker && l.Level == level);
        return relation != null;
    }
    
    public bool IsUserFollowingOtherUser(GameUser follower, GameUser userBeingFollowed)
    {
        int count = _realm.All<FollowRelation>().Count(f => f.Follower == follower && f.Recipient == userBeingFollowed);
        return count > 0;
    }
    
    public void CreatePlay(GameUser user, GameLevel level)
    {
        LevelPlayRelation relation = new (user, level, DateTimeOffset.UtcNow);
        LevelUniquePlayRelation? uniqueRelation = _realm.All<LevelUniquePlayRelation>()
            .FirstOrDefault(r => r.Level == level && r.User == user);

        _realm.Write(() =>
        {
            _realm.Add(relation);
            level.PlaysCount = level.Plays.Count();
            if (uniqueRelation != null) return;
            
            uniqueRelation = new LevelUniquePlayRelation(user, level, DateTimeOffset.UtcNow); 
            _realm.Add(uniqueRelation);
            level.UniquePlaysCount = level.UniquePlays.Count();
            user.PlayedLevelsCount = user.PlayedLevels.Count();
        });
    }
}