namespace SoundShapesServer.Types.ServerResponses.Api.ApiTypes;

public class ApiListInformation
{
    public int TotalItems { get; set; }
    public int? NextPageIndex { get; set; }
    public int? PreviousPageIndex { get; set; }
}