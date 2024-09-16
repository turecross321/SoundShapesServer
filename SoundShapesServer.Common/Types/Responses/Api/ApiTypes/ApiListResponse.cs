using SoundShapesServer.Common.Types.Responses.Api.ApiTypes.Errors;

namespace SoundShapesServer.Common.Types.Responses.Api.ApiTypes;

public class ApiListResponse<T> : ApiResponse<List<T>> where T : class, IApiResponse
{
    public ApiListResponse(IEnumerable<T> data, ApiListInformation info) : base(data.ToList())
    {
        ListInformation = info;
    }

    public ApiListResponse(IEnumerable<T> data) : base(data.ToList())
    {
        ListInformation = null;
    }

    public ApiListResponse(ApiError error) : base(error)
    {
        ListInformation = null;
    }

    public ApiListInformation? ListInformation { get; set; }

    public static implicit operator ApiListResponse<T>(PaginatedList<T> list)
    {
        return new ApiListResponse<T>(list.Items, new ApiListInformation
        {
            TotalItems = list.TotalItems,
            NextPageIndex = GetNextToken(list.TotalItems, list.From, list.Items.Count()),
            PreviousPageIndex = GetPreviousToken(list.TotalItems, list.From, list.Items.Count()),
        });
    }

    public static implicit operator ApiListResponse<T>(ApiError error)
    {
        return new ApiListResponse<T>(error);
    }
    
    private static int? GetPreviousToken(int entryCount, int from, int count)
    {
        int? previousToken;
        if (from > 0) previousToken = Math.Max(from - 1 * entryCount, 0);
        else previousToken = null;
        
        return previousToken;
    }
    
    private static int? GetNextToken(int entryCount, int from, int count)
    {
        int? nextToken;
        if (entryCount <= count + from) nextToken = null;
        else nextToken = count + from;
        
        return nextToken;
    }
}