using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelPublishResponse : IResponse
{
    public LevelPublishResponse(GameLevel level)
    {
        Id = IdHelper.FormatLevelPublishId(level.Id, level.CreationDate.ToUnixTimeMilliseconds());
        Author = new UserTargetResponse(level.Author);
        Title = level.Name;
        Dependencies = new List<string>();
        Visibility = "EVERYONE";
        ExtraData = new LevelExtraDataResponse(level.Language);
        ParentResponse = new LevelParentResponse(level);
        CreationDate = level.CreationDate.ToUnixTimeMilliseconds();
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.PublishedLevel);
    [JsonProperty("author")] public UserTargetResponse Author { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("dependencies")] public IList<string> Dependencies { get; set; }
    [JsonProperty("visibility")] public string Visibility { get; set; }
    [JsonProperty("extraData")] public LevelExtraDataResponse ExtraData { get; set; }
    [JsonProperty("parent")] public LevelParentResponse ParentResponse { get; set; }
    [JsonProperty("creationTime")] public long CreationDate { get; set; }
}