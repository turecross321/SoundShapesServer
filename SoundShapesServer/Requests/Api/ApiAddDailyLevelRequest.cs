namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiAddDailyLevelRequest
{
    public ApiAddDailyLevelRequest(string levelId, DateTimeOffset date)
    {
        LevelId = levelId;
        Date = date;
    }

    public string LevelId { get; }
    public DateTimeOffset Date { get; }
}