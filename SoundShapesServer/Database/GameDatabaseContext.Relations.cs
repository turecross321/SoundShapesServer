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
            follower.FollowingCount = follower.Following.Count();
            recipient.FollowersCount = recipient.Followers.Count();
        });
        
        return true;
    }

    public bool LikeLevel(GameUser user, GameLevel level)
    { 
        if (HasUserLikedLevel(user, level)) return false;
        
        LevelLikeRelation relation = new(DateTimeOffset.UtcNow, user, level);
        
        _realm.Write(() =>
        {
            _realm.Add(relation);
            user.LikedLevelsCount = user.LikedLevels.Count();
            level.LikesCount = level.Likes.Count();
        });
        
        CreateEvent(user, EventType.Like, null, level);

        return true;
    }
    
    public bool UnLikeLevel(GameUser user, GameLevel level)
    {
        if (!HasUserLikedLevel(user, level)) return false;
        
        LevelLikeRelation? relation = _realm.All<LevelLikeRelation>().FirstOrDefault(l => l.User == user && l.Level == level);
        if (relation == null) return false;
        
        _realm.Write(() =>
        {
            _realm.Remove(relation);
            user.LikedLevelsCount = user.LikedLevels.Count();
            level.LikesCount = level.Likes.Count();
        });
        
        return true;
    }

    public bool HasUserLikedLevel(GameUser user, GameLevel level)
    {
        LevelLikeRelation? relation = _realm.All<LevelLikeRelation>().FirstOrDefault(l => l.User == user && l.Level == level);
        return relation != null;
    }
    
    public bool QueueLevel(GameUser user, GameLevel level)
    {
        if (HasUserQueuedLevel(user, level)) return false;
        
        LevelQueueRelation relation = new(DateTimeOffset.UtcNow, user, level);
        
        _realm.Write(() =>
        {
            _realm.Add(relation);
            user.QueuedLevelsCount = user.QueuedLevels.Count();
            level.QueuesCount = level.Queues.Count();
        });
        
        CreateEvent(user, EventType.Queue, null, level);

        return true;
    }
    
    public bool UnQueueLevel(GameUser user, GameLevel level)
    {
        if (!HasUserQueuedLevel(user, level)) return false;
        
        LevelQueueRelation? relation = _realm.All<LevelQueueRelation>().FirstOrDefault(l => l.User == user && l.Level == level);

        if (relation == null) return false;
        
        _realm.Write(() =>
        {
            _realm.Remove(relation);
            user.QueuedLevelsCount = user.QueuedLevels.Count();
            level.QueuesCount = level.Queues.Count();
        });
        
        return true;
    }

    public bool HasUserQueuedLevel(GameUser user, GameLevel level)
    {
        LevelQueueRelation? relation = _realm.All<LevelQueueRelation>().FirstOrDefault(l => l.User == user && l.Level == level);
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