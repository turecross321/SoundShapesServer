using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelPublishResponse
{
    public LevelPublishResponse(GameLevel level)
    {
        Id = IdFormatter.FormatLevelPublishId(level.Id, level.CreationDate.ToUnixTimeMilliseconds());
        Type = GameContentType.upload.ToString();
        Author = new UserResponse(level.Author);
        Title = level.Name;
        Dependencies = new List<string>();
        Visibility = "EVERYONE";
        ExtraData = new ExtraDataResponse(level.Language);
        Parent = new LevelParent(level);
        CreationDate = level.CreationDate.ToUnixTimeMilliseconds();
    }

    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = GameContentType.upload.ToString();
    [JsonProperty("author")] public UserResponse Author { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("dependencies")] public IList<string> Dependencies { get; set; }
    [JsonProperty("visibility")] public string Visibility { get; set; }
    [JsonProperty("extraData")] public ExtraDataResponse ExtraData { get; set; }
    [JsonProperty("parent")] public LevelParent Parent { get; set; }
    [JsonProperty("creationTime")] public long CreationDate { get; set; }
}