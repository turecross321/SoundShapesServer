using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Users;

public class ApiUsersWrapper
{
    public ApiUsersWrapper(IEnumerable<GameUser> users, int count)
    {
        Users = users.Select(u => new ApiUserResponse(u)).ToArray();
        Count = count;
    }

    public ApiUserResponse[] Users { get; set; }
    public int Count { get; set; }
}