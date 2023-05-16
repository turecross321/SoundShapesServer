using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Api.Levels;

public class ApiLevelBriefResponse
{
    public ApiLevelBriefResponse(GameLevel level)
    {
        Id = level.Id;
        Name = level.Name;
        Author = new ApiUserBriefResponse(level.Author);
        Created = level.CreationDate;
        Modified = level.ModificationDate;
        TotalPlays = level.PlaysCount;
        UniquePlays = level.UniquePlaysCount;
        Likes = level.Likes.Count();
        Difficulty = level.Difficulty;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Modified { get; set; }
    public int TotalPlays { get; set; }
    public int UniquePlays { get; set; }
    public int Likes { get; set; }
    public float Difficulty { get; set; }
}