using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUsersWrapper
{
    public ApiUsersWrapper(IEnumerable<GameUser> users, int count)
    {
        Users = users.Select(u => new ApiUserBriefResponse(u)).ToArray();
        Count = count;
    }

    public ApiUserBriefResponse[] Users { get; set; }
    public int Count { get; set; }
}