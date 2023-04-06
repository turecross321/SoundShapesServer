using Bunkum.HttpServer.Authentication;
using Bunkum.HttpServer.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using Realms;
using SoundShapesServer.Enums;
using SoundShapesServer.Type.Relations;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Types;

public class GameUser : RealmObject, IUser
{
    public string id { get; set; } = Guid.NewGuid().ToString();
    public ResponseType type = ResponseType.identity;
    public string display_name { get; set; } = string.Empty;
    
    [JsonIgnore] public ProfileMetadata metadata = new ProfileMetadata();
    
    [Backlink(nameof(LevelLikeRelation.liker))]
    [JsonIgnore] public IQueryable<LevelLikeRelation> likedLevels { get; }
    
    [Backlink(nameof(FollowRelation.userBeingFollowed))]
    [JsonIgnore] public IQueryable<FollowRelation> followers { get; }
    
    [Backlink(nameof(FollowRelation.follower))]
    [JsonIgnore] public IQueryable<FollowRelation> following { get; }
    
    [Backlink(nameof(GameLevel.author))]
    [JsonIgnore] public IQueryable<GameLevel> publishedLevels { get; }
}