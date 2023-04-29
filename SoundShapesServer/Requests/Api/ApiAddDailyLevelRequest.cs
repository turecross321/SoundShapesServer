namespace SoundShapesServer.Requests.Api;

public class ApiAddDailyLevelRequest
{
    public string LevelId { get; set; }
    public DateTimeOffset DateUtc { get; set; }
}