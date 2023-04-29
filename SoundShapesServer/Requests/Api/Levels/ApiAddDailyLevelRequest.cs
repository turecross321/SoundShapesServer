namespace SoundShapesServer.Requests.Api.Levels;

public class ApiAddDailyLevelRequest
{
    public string LevelId { get; set; }
    public DateTimeOffset DateUtc { get; set; }
}