using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelSummaryResponse
{
    public ApiLevelSummaryResponse(GameLevel level)
    {
        Id = level.Id;
        Name = level.Name;
        AuthorId = level.Author?.Id ?? "";
        AuthorName = level.Author?.Username ?? "";
        Created = level.CreationDate;
        Modified = level.ModificationDate;
        TotalPlays = level.Plays;
        UniquePlays = level.UniquePlays.Count;
        Likes = level.Likes.Count();
        Difficulty = level.Difficulty;
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
    public float Difficulty { get; set; }
}