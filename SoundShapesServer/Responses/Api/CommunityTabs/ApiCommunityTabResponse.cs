using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.CommunityTabs;

public class ApiCommunityTabResponse
{
    public ApiCommunityTabResponse(CommunityTab communityTab)
    {
        Id = communityTab.Id;
        Title = communityTab.Title;
        Description = communityTab.Description;
        ButtonLabel = communityTab.ButtonLabel;
        Query = communityTab.Query;
        CreationDate = communityTab.CreationDate;
        ModificationDate = communityTab.ModificationDate;
        Author = new ApiUserBriefResponse(communityTab.Author);
    }

    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ButtonLabel { get; set; }
    public string Query { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }
}