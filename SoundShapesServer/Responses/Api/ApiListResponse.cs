namespace SoundShapesServer.Responses.Api;

public class ApiListResponse<T> : IApiResponse where T : IApiResponse
{
    public ApiListResponse(IEnumerable<T> items, int totalItems)
    {
        Items = items.ToArray();
        Count = totalItems;
    }

    public T[] Items { get; set; }
    public int Count { get; set; }
}