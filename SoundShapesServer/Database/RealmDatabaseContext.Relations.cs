using SoundShapesServer.Responses.Following;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.UserHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public FollowingUsersWrapper? GetFollowers(GameUser userBeingFollowed, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.Recipient == userBeingFollowed);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations 
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        GameUser[] followers = new GameUser[selectedRelations.Length];

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            followers[i] = selectedRelations[i].Follower;
        }

        return UsersToFollowingUsersWrapper(userBeingFollowed, followers, totalEntries, from, count);
    }

    public FollowingUsersWrapper? GetFollowedUsers(GameUser follower, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.Follower == follower);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations      
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        GameUser[] following = new GameUser[selectedRelations.Length];

        for (int i = 0; i < selectedRelations.Count(); i++)
        {
            following[i] = selectedRelations[i].Recipient;
        }

        return UsersToFollowingUsersWrapper(follower, following, totalEntries, from, count);
    }

    public LevelsWrapper? GetUsersLikedLevels(GameUser user, GameUser userToGetLevelsFrom, int from, int count)
    {
        IQueryable<LevelLikeRelation> relations = this._realm.All<LevelLikeRelation>()
            .Where(l => l.Liker == userToGetLevelsFrom);

        int totalEntries = relations.Count();
        
        LevelLikeRelation[] selectedRelations = relations
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        GameLevel[] levels = new GameLevel[selectedRelations.Length];

        for (int i = 0; i < selectedRelations.Length; i++)
        {
            levels[i] = selectedRelations[i].Level;
        }

        return LevelsToLevelsWrapper(levels, user, totalEntries, from, count);
    }

    public bool FollowUser(GameUser follower, GameUser userBeingFollowed)
    {
        if (IsUserFollowingOtherUser(follower, userBeingFollowed)) return false;

        FollowRelation relation = new()
        {
            Follower = follower,
            Recipient = userBeingFollowed
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
        
        FollowRelation? relation = this._realm.All<FollowRelation>().FirstOrDefault(f => f.Follower == follower && f.Recipient == userBeingUnFollowed);

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
            Liker = liker,
            Level = level
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

        LevelLikeRelation? relation = this._realm.All<LevelLikeRelation>().FirstOrDefault(l => l.Liker == liker && l.Level == level);

        if (relation == null) return false;
        
        this._realm.Write(() =>
        {
            this._realm.Remove(relation);
        });
        
        return true;
    }

    public bool IsUserLikingLevel(GameUser liker, GameLevel level)
    {
        int count = this._realm.All<LevelLikeRelation>().Count(l => l.Liker == liker && l.Level == level);
        if (count > 0) return true;
        else return false;
    }
    
    public bool IsUserFollowingOtherUser(GameUser follower, GameUser userBeingFollowed)
    {
        int count = this._realm.All<FollowRelation>().Count(f => f.Follower == follower && f.Recipient == userBeingFollowed);
        if (count > 0) return true;
        else return false;
    }
}