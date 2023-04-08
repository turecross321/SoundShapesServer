using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Following;
using SoundShapesServer.Types;

namespace SoundShapesServer.Helpers;

public static class UserHelper
{
    public static FollowingUsersWrapper UsersToFollowingUsersWrapper
    (GameUser user, GameUser[] users, int totalRelations, int from, int count)
    {
        (int? nextToken, int? previousToken) = PaginationHelper.GetPageTokens(totalRelations, from, count);

        List<FollowingUserResponse> responses = new ();
        
        for (int i = 0; i < users.Length; i++)
        {
            FollowingUserResponse? response = UserToFollowingUserResponse(user, users[i]);
            if (response != null) responses.Add(response);
        }

        FollowingUsersWrapper responsesWrapper = new()
        {
            items = responses.ToArray(),
            nextToken = nextToken,
            previousToken = previousToken
        };

        return responsesWrapper;
    }

    private static FollowingUserResponse? UserToFollowingUserResponse(GameUser? follower, GameUser? followed)
    {
        if (follower == null || followed == null) return null;

        return new FollowingUserResponse
        {
            id = IdFormatter.FormatFollowId(follower.id, followed.id),
            target = UserToUserResponse(followed)
        };
    }
    public static UserResponse? UserToUserResponse(GameUser? user)
    {
        if (user == null) return null;
        
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