using SoundShapesServer.Type.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public IEnumerable<GameUser> GetFollowers(GameUser userBeingFollowed)
    {
        var relations = this._realm.All<FollowRelation>().Where(r => r.userBeingFollowed == userBeingFollowed)
            .ToArray();

        List<GameUser> followers = new List<GameUser>();

        for (int i = 0; i < relations.Length; i++)
        {
            followers.Add(relations[i].follower);
        }

        return followers.AsEnumerable();
    }

    public IEnumerable<GameUser> GetFollowedUsers(GameUser follower)
    {
        var relations = this._realm.All<FollowRelation>().Where(r => r.follower == follower)
            .ToArray();

        List<GameUser> following = new List<GameUser>();

        for (int i = 0; i < relations.Length; i++)
        {
            following.Add(relations[i].userBeingFollowed);
        }

        return following.AsEnumerable();
    }

    public IEnumerable<GameLevel> GetUsersLikedLevels(GameUser user)
    {
        var relations = this._realm.All<LevelLikeRelation>().Where(l => l.liker == user).ToArray();

        List<GameLevel> likedLevels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            likedLevels.Add(relations[i].level);
        }

        return likedLevels.AsEnumerable();
    }
    
    public IEnumerable<GameLevel> GetUsersWhoHaveLikedLevel(GameLevel gameLevel)
    {
        var relations = this._realm.All<LevelLikeRelation>().Where(r => r.level == gameLevel)
            .ToArray();

        List<GameLevel> levels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            levels.Add(relations[i].level);
        }

        return levels.AsEnumerable();
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