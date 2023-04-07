using SoundShapesServer.Responses.Following;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Type.Relations;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;
using static SoundShapesServer.Helpers.LevelHelper;
using static SoundShapesServer.Helpers.UserHelper;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext
{
    public FollowingUserResponsesWrapper? GetFollowers(GameUser userBeingFollowed, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.userBeingFollowed == userBeingFollowed);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations 
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> followers = new ();

        foreach (FollowRelation relation in selectedRelations)
        {
            followers.Add(relation.follower);
        }

        return ConvertUserArrayToFollowingUsersResponsesWrapper(userBeingFollowed, followers.ToArray(), totalEntries, from, count);
    }

    public FollowingUserResponsesWrapper? GetFollowedUsers(GameUser follower, int from, int count)
    {
        IQueryable<FollowRelation> relations = this._realm.All<FollowRelation>().Where(r => r.follower == follower);

        int totalEntries = relations.Count();
        
        FollowRelation[] selectedRelations = relations      
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameUser> following = new ();

        foreach (FollowRelation relation in selectedRelations)
        {
            following.Add(relation.userBeingFollowed);
        }

        return ConvertUserArrayToFollowingUsersResponsesWrapper(follower, following.ToArray(), totalEntries, from, count);
    }

    public LevelResponsesWrapper? GetUsersLikedLevels(GameUser user, int from, int count)
    {
        IQueryable<LevelLikeRelation> relations = this._realm.All<LevelLikeRelation>()
            .Where(l => l.liker == user);

        int totalEntries = relations.Count();
        
        LevelLikeRelation[] selectedRelations = relations
            .AsEnumerable()
            .Skip(from)
            .Take(count)
            .ToArray();

        List<GameLevel> selectedEntries = new ();

        foreach (LevelLikeRelation relation in selectedRelations)
        {
            selectedEntries.Add(relation.level);
        }

        return ConvertGameLevelArrayToLevelResponseWrapper(selectedEntries.ToArray(), totalEntries, from, count);
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