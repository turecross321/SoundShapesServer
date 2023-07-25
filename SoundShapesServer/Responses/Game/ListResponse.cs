using Newtonsoft.Json;
using SoundShapesServer.Helpers;

namespace SoundShapesServer.Responses.Game;

public class ListResponse<T> : IResponse where T : IResponse
{
    public ListResponse(IEnumerable<T> items, int totalItems, int from, int count)
    {
        Items = items.ToArray();
        Count = totalItems;
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalItems, from, count);
    }

    public ListResponse()
    {
        Items = Array.Empty<T>();
        Count = 0;
    }

    [JsonProperty("items")] public T[] Items { get; set; }
    [JsonProperty("count")] public int Count { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}