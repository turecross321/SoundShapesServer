using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext
{
    public IQueryable<GameUser> GetFollowers(GameUser userBeingFollowed)
    {
        IQueryable<FollowRelation> relations = _realm.All<FollowRelation>().Where(r => r.Recipient == userBeingFollowed);

        FollowRelation[] selectedRelations = relations 
            .AsEnumerable()
            .ToArray();

        List<GameUser> followers = selectedRelations.Select(t => t.Follower).ToList();

        return followers.AsQueryable();
    }

    public IQueryable<GameUser> GetFollowedUsers(GameUser follower)
    {
        IQueryable<FollowRelation> relations = _realm.All<FollowRelation>().Where(r => r.Follower == follower);

        FollowRelation[] selectedRelations = relations
            .AsEnumerable()
            .OrderBy(r=>r.Date)
            .ToArray();

        List<GameUser> following = selectedRelations.Select(t => t.Recipient).ToList();

        return following.AsQueryable();
    }

    public IQueryable<GameLevel> GetUsersLikedLevels(GameUser userToGetLevelsFrom)
    {
        List<LevelLikeRelation> relations = _realm.All<LevelLikeRelation>()
            .Where(l => l.Liker == userToGetLevelsFrom)
            .ToList();

        LevelLikeRelation[] selectedRelations = relations.Where(l => l.Liker.Id == userToGetLevelsFrom.Id).ToArray();

        List<GameLevel> levels = selectedRelations.Select(t => t.Level).ToList();

        return levels.AsQueryable();
    }

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
        
        LevelLikeRelation relation = new()
        {
            Date = DateTimeOffset.UtcNow,
            Liker = liker,
            Level = level
        };
        _realm.Write(() =>
        {
            _realm.Add(relation);
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
        int count = _realm.All<LevelLikeRelation>().Count(l => l.Liker == liker && l.Level == level);
        return count > 0;
    }
    
    public bool IsUserFollowingOtherUser(GameUser follower, GameUser userBeingFollowed)
    {
        int count = _realm.All<FollowRelation>().Count(f => f.Follower == follower && f.Recipient == userBeingFollowed);
        return count > 0;
    }
}