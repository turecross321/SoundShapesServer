using Newtonsoft.Json;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelFullResponse
{
    public ApiLevelFullResponse(GameLevel level, GameUser? user)
    {
        bool? completed = null;
        if (user != null) completed = level.UsersWhoHaveCompletedLevel.Contains(user);
        
        Id = level.Id;
        Name = level.Name;
        AuthorId = level.Author.Id;
        AuthorName = level.Author.Username;
        Created = level.CreationDate;
        Modified = level.ModificationDate;
        TotalPlays = level.Plays;
        UniquePlays = level.UniquePlays.Count;
        Likes = level.Likes.Count();
        Deaths = level.Deaths;
        Language = level.Language;
        Difficulty = level.Difficulty;
        AlbumIds = level.Albums.AsEnumerable().Select(a => a.Id).ToArray();
        DailyLevelIds = level.DailyLevels.AsEnumerable().Select(d => d.Id).ToArray();
        CompletedByYou = completed;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string AuthorId { get; set; }
    public string AuthorName { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int TotalPlays { get; set; }
    public int UniquePlays { get; set; }
    public int Likes { get; set; }
    public int Deaths { get; set; }
    public int Language { get; set; }
    public float Difficulty { get; set; }
    public string[] AlbumIds { get; set; }
    public string[] DailyLevelIds { get; set; }
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)] public bool? CompletedByYou { get; set; }
}