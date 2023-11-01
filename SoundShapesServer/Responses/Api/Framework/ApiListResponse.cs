using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Framework;

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
            NextPageIndex = PaginationHelper.GetNextToken(list.TotalItems, list.From, list.Items.Count())
        });
    }

    public static implicit operator ApiListResponse<T>(ApiError error)
    {
        return new ApiListResponse<T>(error);
    }
}