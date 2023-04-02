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
    
    public List<GameUser> GetUsersWhoUserIsFollowing(GameUser follower)
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
    
    public List<GameLevel> GetLevelsFavoritedByUser(GameUser user)
    {
        var relations = this._realm.All<LevelFavoriteRelation>().Where(l => l.user == user)
            .ToArray();

        List<GameLevel> favoritedLevels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            favoritedLevels.Add(relations[i].GameLevel);
        }

        return favoritedLevels;
    }
    
    public List<GameLevel> GetUsersFavoringLevel(GameLevel gameLevel)
    {
        var relations = this._realm.All<LevelFavoriteRelation>().Where(r => r.GameLevel == gameLevel)
            .ToArray();

        List<GameLevel> levels = new List<GameLevel>();

        for (int i = 0; i < relations.Length; i++)
        {
            levels.Add(relations[i].GameLevel);
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
}