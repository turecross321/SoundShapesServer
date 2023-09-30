using System.Net;

namespace SoundShapesServer.Responses.Api.Framework.Errors;

public class ApiUnauthorizedError : ApiError
{
    public const string NoEditPermissionWhen = "You do not have permission to edit the requested resource.";
    public static readonly ApiUnauthorizedError NoEditPermission = new(NoEditPermissionWhen);
    public const string NoDeletionPermissionWhen = "You do not have permission to delete the requested resource.";
    public static readonly ApiUnauthorizedError NoDeletionPermission = new(NoDeletionPermissionWhen);
    public const string NoPermissionWhen = "You do not have permission to do that.";
    public static readonly ApiUnauthorizedError NoPermission = new(NoPermissionWhen);

    public ApiUnauthorizedError(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}