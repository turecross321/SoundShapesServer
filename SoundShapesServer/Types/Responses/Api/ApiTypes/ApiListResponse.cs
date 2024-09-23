using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiListResponse<TResponse> : ApiResponse<IEnumerable<TResponse>>
{
    public static ApiListResponse<TDbResponse> FromPaginatedList<TDb, TDbId, TDbResponse>(PaginatedDbList<TDb, TDbId> paginatedList) 
        where TDb : IDbItem<TDbId>
        where TDbResponse : IApiDbResponse<TDb, TDbResponse>, IApiResponse
    {
        return new ApiListResponse<TDbResponse>(paginatedList.Items.Select(TDbResponse.FromDb))
        {
            ListInformation = new ApiListInformation
            {
                TotalItems = paginatedList.TotalItems,
                NextPageIndex = paginatedList.NextPageIndex(),
                PreviousPageIndex = paginatedList.PreviousPageIndex(),
            },
        };
    }
    
    
    private ApiListResponse(ApiError error) : base(error)
    {
        
    }
    
    private ApiListResponse(IEnumerable<TResponse> items) : base(items)
    {
    }
    
    [Obsolete("Only meant to be used for documentation.")]
    public ApiListResponse() {}

    public ApiListInformation? ListInformation { get; set; }
    
    public static implicit operator ApiListResponse<TResponse>(ApiError error)
    {
        return new ApiListResponse<TResponse>(error)
        {
            ListInformation = null,
        };
    }
    
    public static implicit operator ApiListResponse<TResponse>(List<TResponse> list)
    {
        return new ApiListResponse<TResponse>(list)
        {
            ListInformation = null,
        };
    }
}