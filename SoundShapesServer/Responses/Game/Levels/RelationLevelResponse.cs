using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Levels;

public class RelationLevelResponse : IResponse
{
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Queued);

    public RelationLevelResponse(ILevelRelation relation, GameUser accessor)
    {
        Id = IdHelper.FormatRelationLevelId(relation.User.Id, relation.Level.Id);
        Timestamp = relation.Date.ToUnixTimeMilliseconds();
        Level = new LevelTargetResponse(relation.Level, accessor);
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("timestamp")] public long Timestamp { get; set; }
    [JsonProperty("target")] public LevelTargetResponse Level { get; set; }
}