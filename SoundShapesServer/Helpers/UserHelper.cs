using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public class UserHelper
{
    public static UserResponse GetUserResponseFromGameUser(GameUser user)
    {
        string formattedAuthorId = IdFormatter.FormatUserId(user.id);

        return new UserResponse
        {
            id = formattedAuthorId,
            type = ResponseType.identity.ToString(),
            displayName = user.display_name
        };
    }
}