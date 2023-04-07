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
    (GameUser follower, GameUser[] followedUsers, int totalRelations, int from, int count)
    {
        (int? nextToken, int? previousToken) = PaginationHelper.GetPageTokens(totalRelations, from, count);

        FollowingUserResponse[] responses = new FollowingUserResponse[followedUsers.Length];
        
        for (int i = 0; i < followedUsers.Length; i++)
        {
            FollowingUserResponse response = new()
            {
                id = IdFormatter.FormatFollowId(follower.id, followedUsers[i].id),
                target = new UserResponse()
                {
                    id = IdFormatter.FormatUserId(followedUsers[i].id),
                    type = ResponseType.identity.ToString(),
                    displayName = followedUsers[i].display_name
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