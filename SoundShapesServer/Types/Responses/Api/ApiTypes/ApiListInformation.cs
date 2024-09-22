namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiListInformation
{
    public required int TotalItems { get; set; }
    public required int? NextPageIndex { get; set; }
    public required int? PreviousPageIndex { get; set; }
    
}