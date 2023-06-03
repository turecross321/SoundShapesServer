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
        CreationDate = level.CreationDate;
        ModificationDate = level.ModificationDate;
        TotalPlays = level.PlaysCount;
        UniquePlays = level.UniquePlaysCount;
        Likes = level.Likes.Count();
        Queues = level.Queues.Count();
        Difficulty = level.Difficulty;
    }
    
#pragma warning disable CS8618
    public ApiLevelBriefResponse() {}
#pragma warning restore CS8618

    public string Id { get; set; }
    public string Name { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
    public int TotalPlays { get; set; }
    public int UniquePlays { get; set; }
    public int Likes { get; set; }
    public int Queues { get; set; }
    public float Difficulty { get; set; }
}