using Newtonsoft.Json;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int TotalPlays { get; set; }
    public int UniquePlays { get; set; }
    public int Likes { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public bool? CompletedByYou { get; set; }
}