using Newtonsoft.Json;
using SoundShapesServer.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Levels;
public class LevelPublishResponse
{
    [JsonProperty("id")] public string Id { get; set; }
    [JsonProperty("type")] public string Type = ResponseType.upload.ToString();
    [JsonProperty("author")] public UserResponse Author { get; set; }
    [JsonProperty("title")] public string Title { get; set; }
    [JsonProperty("dependencies")] public IList<string> Dependencies { get; set; }
    [JsonProperty("visibility")] public string Visibility { get; set; }
    [JsonProperty("description")] public string Description { get; set; }
    [JsonProperty("extraData")] public ExtraDataResponse ExtraData { get; set; }
    [JsonProperty("parent")] public LevelParent Parent { get; set; }
    [JsonProperty("creationTime")] public long CreationDate { get; set; }
}