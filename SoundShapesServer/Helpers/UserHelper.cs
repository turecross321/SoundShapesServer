using Bunkum.HttpServer;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    public static FollowingUserResponsesWrapper? ConvertUserArrayToFollowingUsersResponsesWrapper
    (GameUser user, GameUser[] users, int totalRelations, int from, int count)
    {
        (int? nextToken, int? previousToken) = PaginationHelper.GetPageTokens(totalRelations, from, count);

        FollowingUserResponse[] responses = new FollowingUserResponse[users.Length];
        
        for (int i = 0; i < users.Length; i++)
        {
            FollowingUserResponse response = new()
            {
                id = IdFormatter.FormatFollowId(user.id, users[i].id),
                target = new UserResponse()
                {
                    id = IdFormatter.FormatUserId(users[i].id),
                    type = ResponseType.identity.ToString(),
                    displayName = users[i].display_name
                }
            };
            
            responses[i] = response;
        }

        FollowingUserResponsesWrapper responsesWrapper = new()
        {
            items = responses,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return responsesWrapper;
    }
    public static UserResponse ConvertGameUserToUserResponse(GameUser user)
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