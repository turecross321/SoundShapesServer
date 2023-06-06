// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiCreateNewsEntryRequest
{
    public string? Language { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? FullText { get; set; }
    public string? Url { get; set; }
}