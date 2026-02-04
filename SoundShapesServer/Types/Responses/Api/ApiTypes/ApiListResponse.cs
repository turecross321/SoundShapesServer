using System.Net;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes;

public class ApiListResponse<TResponse> : ApiResponse<IEnumerable<TResponse>>
{
    public static ApiListResponse<TDbResponse> FromPaginatedList<TDb, TDbId, TDbResponse>(
        PaginatedDbList<TDb, TDbId> paginatedList)
        where TDb : IDbItem<TDbId>
        where TDbResponse : IApiDbResponse<TDb, TDbResponse>
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
    
    public ApiListResponse(IEnumerable<TResponse> items) : base(items)
    {
    }
    
    public ApiListResponse(IEnumerable<TResponse> items, HttpStatusCode code) : base(items, code)
    {
    }
    
    [Obsolete("Only meant to be used for deserialization and documentation.")]
    public ApiListResponse()
    {
    }
    
    public ApiListInformation ListInformation { get; set; } = new ApiListInformation
    {
        TotalItems = null,
        NextPageIndex = null,
        PreviousPageIndex = null,
    };
    
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