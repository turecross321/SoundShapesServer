namespace SoundShapesServer.Responses.Api;

public class ApiListResponse<T> : IApiResponse where T : IApiResponse
{
    public ApiListResponse(IEnumerable<T> items, int totalItems)
    {
        Items = items.ToList();
        Count = totalItems;
    }

    public List<T> Items { get; set; }
    public int Count { get; set; }
}