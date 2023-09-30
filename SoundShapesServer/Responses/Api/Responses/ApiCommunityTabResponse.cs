using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Responses;

public class ApiCommunityTabResponse : IApiResponse
{
    public ApiCommunityTabResponse(CommunityTab communityTab)
    {
        Id = communityTab.Id;
        ContentType = communityTab.ContentType;
        Title = communityTab.Title;
        Description = communityTab.Description;
        ButtonLabel = communityTab.ButtonLabel;
        Query = communityTab.Query;
        CreationDate = communityTab.CreationDate.ToUnixTimeSeconds();
        ModificationDate = communityTab.ModificationDate.ToUnixTimeSeconds();
        Author = new ApiUserBriefResponse(communityTab.Author);
    }

    public string Id { get; set; }
    public GameContentType ContentType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonLabel { get; set; }
    public string Query { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public long CreationDate { get; set; }
    public long ModificationDate { get; set; }
}