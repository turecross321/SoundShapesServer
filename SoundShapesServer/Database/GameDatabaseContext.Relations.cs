using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    #region User Following
    public bool FollowUser(GameUser follower, GameUser recipient, PlatformType platformType)
    {
        if (IsUserFollowingOtherUser(follower, recipient)) 
            return false;

        UserFollowRelation relation = new()
        {
            Date = DateTimeOffset.UtcNow,
            Follower = follower,
            Recipient = recipient
        };
        _realm.Write(() =>
        {
            _realm.Add(relation);
            follower.FollowingCount = follower.FollowingRelations.Count();
            recipient.FollowersCount = recipient.FollowersRelations.Count();
        });

        CreateEvent(follower, EventType.UserFollow, platformType, EventDataType.User, relation.Recipient.Id);
        
        return true;
    }
    
    public bool UnFollowUser(GameUser follower, GameUser recipient)
    {
        if (!IsUserFollowingOtherUser(follower, recipient)) return false;
        
        UserFollowRelation? relation = _realm.All<UserFollowRelation>().FirstOrDefault(f => f.Follower == follower && f.Recipient == recipient);

        if (relation == null) return false;
        
        _realm.Write(() =>
        {
            _realm.Remove(relation);
            follower.FollowingCount = follower.FollowingRelations.Count();
            recipient.FollowersCount = recipient.FollowersRelations.Count();
        });
        
        return true;
    }
    
    public bool IsUserFollowingOtherUser(GameUser follower, GameUser userBeingFollowed)
    {
        int count = _realm.All<UserFollowRelation>().Count(f => f.Follower == follower && f.Recipient == userBeingFollowed);
        return count > 0;
    }
    
    #endregion

    #region Level Liking
    public bool LikeLevel(GameUser user, GameLevel level, PlatformType platformType)
    { 
        if (HasUserLikedLevel(user, level)) return false;

        LevelLikeRelation relation = new LevelLikeRelation()
        {
            Date = DateTimeOffset.UtcNow,
            User = user,
            Level = level
        };
        
        _realm.Write(() =>
        {
            _realm.Add(relation);
            user.LikedLevelsCount = user.LikedLevelRelations.Count();
            level.LikesCount = level.Likes.Count();
        });
        
        CreateEvent(user, EventType.LevelLike, platformType, EventDataType.Level, relation.Level.Id);

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
            user.LikedLevelsCount = user.LikedLevelRelations.Count();
            level.LikesCount = level.Likes.Count();
        });
        
        return true;
    }

    public bool HasUserLikedLevel(GameUser user, GameLevel level)
    {
        LevelLikeRelation? relation = _realm.All<LevelLikeRelation>().FirstOrDefault(l => l.User == user && l.Level == level);
        return relation != null;
    }
    
    #endregion
    
    #region Level Queuing
    public bool QueueLevel(GameUser user, GameLevel level, PlatformType platformType)
    {
        if (HasUserQueuedLevel(user, level)) return false;

        LevelQueueRelation relation = new LevelQueueRelation()
        {
            Date = DateTimeOffset.UtcNow,
            Level = level,
            User = user
        };
        
        _realm.Write(() =>
        {
            _realm.Add(relation);
            user.QueuedLevelsCount = user.QueuedLevelRelations.Count();
            level.QueuesCount = level.Queues.Count();
        });
        
        CreateEvent(user, EventType.LevelQueue, platformType, EventDataType.Level, relation.Level.Id);

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
            user.QueuedLevelsCount = user.QueuedLevelRelations.Count();
            level.QueuesCount = level.Queues.Count();
        });
        
        return true;
    }

    public bool HasUserQueuedLevel(GameUser user, GameLevel level)
    {
        LevelQueueRelation? relation = _realm.All<LevelQueueRelation>().FirstOrDefault(l => l.User == user && l.Level == level);
        return relation != null;
    }
    #endregion

    #region Level Plays
    public void CreatePlay(GameUser user, GameLevel level)
    {
        LevelPlayRelation relation = new LevelPlayRelation
        {
            User = user, 
            Level = level, 
            Date = DateTimeOffset.UtcNow
        };
        LevelUniquePlayRelation? uniqueRelation = _realm.All<LevelUniquePlayRelation>()
            .FirstOrDefault(r => r.Level == level && r.User == user);

        _realm.Write(() =>
        {
            _realm.Add(relation);
            level.PlaysCount = level.Plays.Count();
            if (uniqueRelation != null) return;
            
            uniqueRelation = new LevelUniquePlayRelation()
            {
                Date = DateTimeOffset.UtcNow,
                Level = level,
                User = user
            }; 
            _realm.Add(uniqueRelation);
            level.UniquePlaysCount = level.UniquePlays.Count();
            user.PlayedLevelsCount = user.PlayedLevelRelations.Count();
        });
    }
    
    #endregion
}