using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public List<GameUser> GetFollowers(GameUser userBeingFollowed)
    {
        var relations = this._realm.All<FollowRelation>().Where(r => r.userBeingFollowed == userBeingFollowed)
            .ToArray();

        List<GameUser> followers = new List<GameUser>();

        for (int i = 0; i < relations.Length; i++)
        {
            followers.Add(relations[i].follower);
        }

        return followers;
    }

    public List<GameUser> GetFollowings(GameUser follower)
    {
        var relations = this._realm.All<FollowRelation>().Where(r => r.follower == follower)
            .ToArray();

        List<GameUser> following = new List<GameUser>();

        for (int i = 0; i < relations.Length; i++)
        {
            following.Add(relations[i].userBeingFollowed);
        }

        return following;
    }

    public List<GameLevel> GetUsersLikedLevels(GameUser user)
    {
        var relations = this._realm.All<LevelLikeRelation>().Where(l => l.liker == user)
            .ToArray();

        List<GameLevel> favoritedLevels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            favoritedLevels.Add(relations[i].level);
        }

        return favoritedLevels;
    }
    
    public List<GameLevel> GetUsersWhoHaveLikedLevel(GameLevel gameLevel)
    {
        var relations = this._realm.All<LevelLikeRelation>().Where(r => r.level == gameLevel)
            .ToArray();

        List<GameLevel> levels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            levels.Add(relations[i].level);
        }

        return levels;
    }

    public bool FollowUser(GameUser follower, GameUser userBeingFollowed)
    {
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

    public bool LikeLevel(GameUser liker, GameLevel level)
    {
        if (liker == null || level == null) return false;
        if (HasUserLikedLevel(liker, level)) return false; 
        
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
        if (liker == null || level == null) return false;

        LevelLikeRelation? relation = this._realm.All<LevelLikeRelation>().Where(l => l.liker == liker && l.level == level).FirstOrDefault();

        if (relation == null) return false;
        
        this._realm.Write(() =>
        {
            this._realm.Remove(relation);
        });
        
        return true;
    }

    public bool HasUserLikedLevel(GameUser liker, GameLevel level)
    {
        int count = this._realm.All<LevelLikeRelation>().Count(l => l.liker == liker && l.level == level);
        if (count > 0) return true;
        else return false;
    }
}