using System.Net;

namespace SoundShapesServer.Common.Types.Responses.Api.ApiTypes.Errors;

public class ApiConflictError : ApiError
{
    public const string UsernameAlreadyTakenWhen = "Username already taken.";
    public static readonly ApiConflictError UsernameAlreadyTaken = new(UsernameAlreadyTakenWhen);
    public const string EmailAlreadyTakenWhen = "Email is already in use.";
    public static readonly ApiConflictError EmailAlreadyTaken = new(EmailAlreadyTakenWhen);
    public const string TooManyCommunityTabsWhen = "There can't be more than 4 custom community tabs at a time.";
    public static readonly ApiConflictError TooManyCommunityTabs = new(TooManyCommunityTabsWhen);
    public const string AlreadyLikedLevelWhen = "You have already liked this level.";
    public static readonly ApiConflictError AlreadyLikedLevel = new(AlreadyLikedLevelWhen);
    public const string AlreadyQueuedLevelWhen = "You have already queued this level.";
    public static readonly ApiConflictError AlreadyQueuedLevel = new(AlreadyQueuedLevelWhen);
    public const string AlreadyFollowingWhen = "You are already following this user.";
    public static readonly ApiConflictError AlreadyFollowing = new(AlreadyFollowingWhen);

    private ApiConflictError(string message) : base(message, HttpStatusCode.Conflict)
    {
    }
}