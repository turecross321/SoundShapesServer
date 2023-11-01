using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.News;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiNewsEntryResponse : IApiResponse, IDataConvertableFrom<ApiNewsEntryResponse, NewsEntry>
{
    public required string Id { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required string Language { get; set; }
    public required string Title { get; set; }
    public required string Summary { get; set; }
    public required string FullText { get; set; }
    public required string Url { get; set; }

    public static ApiNewsEntryResponse FromOld(NewsEntry old)
    {
        return new ApiNewsEntryResponse
        {
            Id = old.Id.ToString()!,
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            Language = old.Language,
            Author = ApiUserBriefResponse.FromOld(old.Author),
            Title = old.Title,
            Summary = old.Summary,
            FullText = old.FullText,
            Url = old.Url
        };
    }

    public static IEnumerable<ApiNewsEntryResponse> FromOldList(IEnumerable<NewsEntry> oldList)
    {
        return oldList.Select(FromOld);
    }
}