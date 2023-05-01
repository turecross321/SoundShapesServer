namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAddDailyLevelRequest
{
    public ApiAddDailyLevelRequest(string levelId, DateTimeOffset dateUtc)
    {
        LevelId = levelId;
        DateUtc = dateUtc;
    }

    public string LevelId { get; }
    public DateTimeOffset DateUtc { get; }
}