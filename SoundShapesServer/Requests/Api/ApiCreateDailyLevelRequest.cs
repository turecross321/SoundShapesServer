#pragma warning disable CS8618
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateDailyLevelRequest
{
    public string LevelId { get; set; }
    public long Date { get; set; }
}