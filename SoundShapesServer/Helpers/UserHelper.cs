using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    public static FollowingUsersWrapper? UsersToFollowingUsersWrapper
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

        FollowingUsersWrapper responsesWrapper = new()
        {
            items = responses,
            nextToken = nextToken,
            previousToken = previousToken
        };

        return responsesWrapper;
    }
    public static UserResponse UserToUserResponse(GameUser user)
    {
        string formattedAuthorId = IdFormatter.FormatUserId(user.id);

        return new UserResponse
        {
            id = formattedAuthorId,
            type = ResponseType.identity.ToString(),
            displayName = user.display_name
        };
    }

    public static UserMetadataResponse GenerateUserMetadata(GameUser user)
    {
        return new UserMetadataResponse()
        {
            displayName = user.display_name,
            follows_of_ever_count = user.followers.Count(), // Followers
            levels_by_ever_count = user.publishedLevels.Count(), // Level Count
            follows_by_ever_count = user.following.Count(), // Following
            likes_by_ever_count = user.likedLevels.Count(), // Liked And Queued Levels
        };
    }
}