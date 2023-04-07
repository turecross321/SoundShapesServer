using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using Realms;
using SoundShapesServer.Enums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public ResponseType type = ResponseType.identity;
    public string display_name { get; set; } = string.Empty;

    [Backlink(nameof(LevelLikeRelation.liker))] public IQueryable<LevelLikeRelation> likedLevels { get; }
    
    [Backlink(nameof(FollowRelation.userBeingFollowed))] public IQueryable<FollowRelation> followers { get; }
    
    [Backlink(nameof(FollowRelation.follower))] public IQueryable<FollowRelation> following { get; }
    
    [Backlink(nameof(GameLevel.author))] public IQueryable<GameLevel> publishedLevels { get; }
}