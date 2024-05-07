#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiDailyLevelRequest
{
    public string LevelId { get; set; }
    public DateTimeOffset Date { get; set; }
}