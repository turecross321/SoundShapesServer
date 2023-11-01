using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiCommunityTabResponse : IApiResponse, IDataConvertableFrom<ApiCommunityTabResponse, CommunityTab>
{
    public required string Id { get; set; }
    public required GameContentType ContentType { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string ButtonLabel { get; set; }
    public required string Query { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }

    public static ApiCommunityTabResponse FromOld(CommunityTab old)
    {
        return new ApiCommunityTabResponse
        {
            Id = old.Id.ToString()!,
            ContentType = old.ContentType,
            Title = old.Title,
            Description = old.Description,
            ButtonLabel = old.ButtonLabel,
            Query = old.Query,
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            Author = ApiUserBriefResponse.FromOld(old.Author)
        };
    }

    public static IEnumerable<ApiCommunityTabResponse> FromOldList(IEnumerable<CommunityTab> oldList)
    {
        return oldList.Select(FromOld);
    }
}