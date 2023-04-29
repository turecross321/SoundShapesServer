namespace SoundShapesServer.Responses.Api.Levels;

public class ApiDailyLevelResponse
{
    public string Id { get; set; }
    public string LevelId { get; set; }
    public DateTimeOffset DateUtc { get; set; }
}