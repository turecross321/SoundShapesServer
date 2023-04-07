using SoundShapesServer.Helpers;
using SoundShapesServer.Type.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public (GameUser[], int) GetFollowers(GameUser userBeingFollowed, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.userBeingFollowed == userBeingFollowed);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations 
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> followers = new List<GameUser>();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            followers.Add(selectedRelations[i].follower);
        }

        return (followers.ToArray(), totalEntries);
    }

    public (GameUser[], int) GetFollowedUsers(GameUser follower, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.follower == follower);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations      
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> following = new List<GameUser>();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            following.Add(selectedRelations[i].userBeingFollowed);
        }

        return (following.ToArray(), totalEntries);
    }

    public (GameLevel[], int) GetUsersLikedLevels(GameUser user, int from, int count)
    {
        IQueryable<LevelLikeRelation> relations = this._realm.All<LevelLikeRelation>()
            .Where(l => l.liker == user);

        int totalEntries = relations.Count();
        
        LevelLikeRelation[] selectedRelations = relations
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameLevel> entries = new ();

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            entries.Add(selectedRelations[i].level);
        }

        return (entries.ToArray(), totalEntries);
    }

    public bool FollowUser(GameUser follower, GameUser userBeingFollowed)
    {
        if (IsUserFollowingOtherUser(follower, userBeingFollowed)) return false;

        FollowRelation relation = new()
        {
            follower = follower,
            userBeingFollowed = userBeingFollowed
        };
        this._realm.Write(() =>
        {
            this._realm.Add(relation);
        });

        return true;
    }
    
    public bool UnFollowUser(GameUser follower, GameUser userBeingUnFollowed)
    {
        if (!IsUserFollowingOtherUser(follower, userBeingUnFollowed)) return false;
        
        FollowRelation? relation = this._realm.All<FollowRelation>().FirstOrDefault(f => f.follower == follower && f.userBeingFollowed == userBeingUnFollowed);

        if (relation == null) return false;
        
        this._realm.Write(() =>
        {
            this._realm.Remove(relation);
        });
        
        return true;
    }

    public bool LikeLevel(GameUser liker, GameLevel level)
    {
        if (IsUserLikingLevel(liker, level)) return false; 
        
        LevelLikeRelation relation = new()
        {
            liker = liker,
            level = level
        };
        this._realm.Write(() =>
        {
            this._realm.Add(relation);
        });

        return true;
    }
    
    public bool UnLikeLevel(GameUser liker, GameLevel level)
    {
        if (!IsUserLikingLevel(liker, level)) return false;

        LevelLikeRelation? relation = this._realm.All<LevelLikeRelation>().FirstOrDefault(l => l.liker == liker && l.level == level);

        if (relation == null) return false;
        
        this._realm.Write(() =>
        {
            this._realm.Remove(relation);
        });
        
        return true;
    }

    public bool IsUserLikingLevel(GameUser liker, GameLevel level)
    {
        int count = this._realm.All<LevelLikeRelation>().Count(l => l.liker == liker && l.level == level);
        if (count > 0) return true;
        else return false;
    }
    
    public bool IsUserFollowingOtherUser(GameUser follower, GameUser userBeingFollowed)
    {
        int count = this._realm.All<FollowRelation>().Count(f => f.follower == follower && f.userBeingFollowed == userBeingFollowed);
        if (count > 0) return true;
        else return false;
    }
}