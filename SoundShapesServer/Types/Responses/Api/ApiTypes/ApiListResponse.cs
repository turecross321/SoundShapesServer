using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiListResponse<TResponse> : ApiResponse<List<TResponse>> where TResponse : IApiResponse
{
    public static ApiListResponse<TDbResponse> FromPaginatedList<TDb, TDbId, TDbResponse>(PaginatedDbList<TDb, TDbId> paginatedList) 
        where TDb : IDbItem<TDbId>
        where TDbResponse : IApiDbResponse<TDb, TDbResponse>
    {
        return new ApiListResponse<TDbResponse>(paginatedList.Items.Select(TDbResponse.FromDb).ToList())
        {
            ListInformation = new ApiListInformation
            {
                TotalItems = paginatedList.TotalItems,
                NextPageIndex = paginatedList.NextPageIndex(),
                PreviousPageIndex = paginatedList.PreviousPageIndex(),
            }
        };
    }
    
    
    public ApiListResponse(ApiError error) : base(error)
    {
        
    }

    public ApiListResponse(List<TResponse> items) : base(items)
    {
        
    }

    public required ApiListInformation? ListInformation { get; set; }
    
    public static implicit operator ApiListResponse<TResponse>(ApiError error)
    {
        return new ApiListResponse<TResponse>(error)
        {
            ListInformation = null
        };
    }

}