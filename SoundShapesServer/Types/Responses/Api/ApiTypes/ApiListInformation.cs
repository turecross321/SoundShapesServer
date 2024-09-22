namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiListInformation
{
    public required int TotalItems { get; set; }
    public required string? NextPageItemId { get; set; }
    public required int? NextPageIndex { get; set; }
    public required int? PreviousPageIndex { get; set; }
    public required string? PreviousPageItemId { get; set; }
    
}