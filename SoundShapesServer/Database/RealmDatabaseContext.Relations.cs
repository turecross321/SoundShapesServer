using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IQueryable<GameUser> GetFollowers(GameUser userBeingFollowed, int from, int count)
    {
        IQueryable<FollowRelation> relations = _realm.All<FollowRelation>().Where(r => r.Recipient == userBeingFollowed);

        FollowRelation[] selectedRelations = relations 
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> followers = new ();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            GameUser? gameUser = selectedRelations[i].Follower;
            if (gameUser != null) followers.Add(gameUser);
        }

        return followers.AsQueryable();
    }

    public IQueryable<GameUser> GetFollowedUsers(GameUser follower, int from, int count)
    {
        IQueryable<FollowRelation> relations = _realm.All<FollowRelation>().Where(r => r.Follower == follower);

        FollowRelation[] selectedRelations = relations      
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> following = new();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            GameUser? gameUser = selectedRelations[i].Recipient;
            if (gameUser != null) following.Add(gameUser);
        }

        return following.AsQueryable();
    }

    public IQueryable<GameLevel> GetUsersLikedLevels(GameUser userToGetLevelsFrom)
    {
        List<LevelLikeRelation> relations = _realm.All<LevelLikeRelation>()
            .Where(l => l.Liker == userToGetLevelsFrom)
            .ToList();

        LevelLikeRelation[] selectedRelations = relations.Where(l => l.Liker?.Id == userToGetLevelsFrom.Id).ToArray();

        List<GameLevel> levels = new ();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            GameLevel? level = selectedRelations[i].Level;
            if (level != null) levels.Add(level);
        }
        
        return levels.AsQueryable();
    }

    public bool FollowUser(GameUser follower, GameUser recipient)
    {
        if (IsUserFollowingOtherUser(follower, recipient)) return false;

        FollowRelation relation = new()
        {
            Follower = follower,
            Recipient = recipient
        };
        _realm.Write(() =>
        {
            _realm.Add(relation);
        });

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
            Liker = liker,
            Level = level
        };
        _realm.Write(() =>
        {
            _realm.Add(relation);
        });

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