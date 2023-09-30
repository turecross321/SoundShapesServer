using SoundShapesServer.Responses.Api.Framework.Errors;

namespace SoundShapesServer.Responses.Api.Framework;

public class ApiListResponse<T> : ApiResponse<List<T>> where T : class, IApiResponse
{
    public ApiListResponse(IEnumerable<T> items, int totalItems) : base(items.ToList())
    {
        ListInformation = new ApiListInformation
        {
            TotalItems = totalItems
        };
    }
    public ApiListResponse(ApiError error) : base(error)
    {
        this.ListInformation = null;
    }
    
    public static implicit operator ApiListResponse<T>(ApiError error)
    {
        return new ApiListResponse<T>(error);
    }
    
    public ApiListInformation? ListInformation { get; set; }
}