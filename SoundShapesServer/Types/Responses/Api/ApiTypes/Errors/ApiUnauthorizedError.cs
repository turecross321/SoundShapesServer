using System.Net;

namespace SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;

public class ApiUnauthorizedError : ApiError
{
    public const string NoEditPermissionWhen = "You do not have permission to edit the requested resource.";
    public static readonly ApiUnauthorizedError NoEditPermission = new(NoEditPermissionWhen);
    public const string NoDeletionPermissionWhen = "You do not have permission to delete the requested resource.";
    public static readonly ApiUnauthorizedError NoDeletionPermission = new(NoDeletionPermissionWhen);
    public const string NoPermissionWhen = "You do not have permission to do that.";
    public static readonly ApiUnauthorizedError NoPermission = new(NoPermissionWhen);
    public const string EulaNotAcceptedWhen = "You have to accept the eula to register an account.";
    public static readonly ApiUnauthorizedError EulaNotAccepted = new(EulaNotAcceptedWhen);
    public const string InvalidCodeWhen = "Invalid code.";
    public static readonly ApiUnauthorizedError InvalidCode = new(InvalidCodeWhen);
    public const string InvalidEmailOrPasswordWhen = "Invalid e-mail or password.";
    public static readonly ApiUnauthorizedError InvalidEmailOrPassword = new(InvalidEmailOrPasswordWhen);

    private ApiUnauthorizedError(string message) : base(message, HttpStatusCode.Unauthorized)
    {
    }
}