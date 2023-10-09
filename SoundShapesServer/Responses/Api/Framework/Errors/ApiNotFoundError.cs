using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiNotFoundError : ApiError
{
    public const string LevelNotFoundWhen = "Could not find level with specified ID.";
    public static readonly ApiNotFoundError LevelNotFound = new(LevelNotFoundWhen);
    public const string DailyLevelNotFoundWhen = "Could not find daily level pick with specified ID.";
    public static readonly ApiNotFoundError DailyLevelNotFound = new(DailyLevelNotFoundWhen);
    public const string AlbumNotFoundWhen = "Could not find album with specified ID.";
    public static readonly ApiNotFoundError AlbumNotFound = new(AlbumNotFoundWhen);
    public const string UserNotFoundWhen = "Could not find specified user.";
    public static readonly ApiNotFoundError UserNotFound = new(UserNotFoundWhen);
    public const string CommunityTabNotFoundWhen = "Could not find community tab with specified ID.";
    public static readonly ApiNotFoundError CommunityTabNotFound = new(CommunityTabNotFoundWhen);
    public const string EventNotFoundWhen = "Could not find event with specified ID.";
    public static readonly ApiNotFoundError EventNotFound = new(EventNotFoundWhen);
    public const string LeaderboardEntryNotFoundWhen = "Could not find leaderboard entry with specified ID.";
    public static readonly ApiNotFoundError LeaderboardEntryNotFound = new(LeaderboardEntryNotFoundWhen);
    public const string NewsEntryNotFoundWhen = "Could not find news entry with specified ID.";
    public static readonly ApiNotFoundError NewsEntryNotFound = new(NewsEntryNotFoundWhen);
    public const string ReportNotFoundWhen = "Could not find report with specified ID.";
    public static readonly ApiNotFoundError ReportNotFound = new(ReportNotFoundWhen);
    public const string PunishmentNotFoundWhen = "Could not find punishment with specified ID.";
    public static readonly ApiNotFoundError PunishmentNotFound = new(PunishmentNotFoundWhen);
    public const string FileDoesNotExistWhen = "File does not exist.";
    public static readonly ApiNotFoundError FileDoesNotExist = new(FileDoesNotExistWhen);
    public const string RefreshTokenDoesNotExistWhen = "Refresh token with specified ID does not exist. It has probably expired.";
    public static readonly ApiNotFoundError RefreshTokenDoesNotExist = new(RefreshTokenDoesNotExistWhen);
    public const string TokenDoesNotExistWhen = "Token with specified ID does not exist.";
    public static readonly ApiNotFoundError TokenDoesNotExist = new(TokenDoesNotExistWhen);
    
    public const string NotLikedLevelWhen = "You have not liked this level.";
    public static readonly ApiNotFoundError NotLikedLevel = new(NotLikedLevelWhen);
    public const string NotQueuedLevelWhen = "You have not queued this level.";
    public static readonly ApiNotFoundError NotQueuedLevel = new(NotQueuedLevelWhen);
    public const string NotFollowingWhen = "You are not following this user.";
    public static readonly ApiNotFoundError NotFollowing = new(NotFollowingWhen);

    public ApiNotFoundError(string message) : base(message, HttpStatusCode.NotFound)
    {
    }
}