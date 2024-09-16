using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.RateLimit;
using Bunkum.Protocols.Http;
using SoundShapesServer.Common.Verification;
using SoundShapesServer.Database;
using SoundShapesServer.Services;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Config;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;
using SoundShapesServer.Types.ServerResponses.Api.ApiTypes;
using SoundShapesServer.Types.ServerResponses.Api.ApiTypes.Errors;
using SoundShapesServer.Types.ServerResponses.Api.DataTypes;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAccountCredentialEndpoints : EndpointGroup
{
    /// <remarks>
    ///     If increased, passwords will automatically be rehashed at login time to use the new WorkFactor 
    ///     If decreased, passwords will stay at higher WorkFactor until reset
    /// </remarks>
    private const int WorkFactor = 14;
    
    /// <summary>
    /// A randomly generated password.
    /// Used to prevent against timing attacks.
    /// </summary>
    private static readonly string FakePassword = BCrypt.Net.BCrypt.HashPassword(Random.Shared.Next().ToString(), WorkFactor);
    
    [DocSummary("Retrieves the user that the specified set email token belongs to.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.TokenDoesNotExistWhen)]
    [ApiEndpoint("codeToken/{code}/owner")]
    [Authentication(false)]
    public ApiResponse<ApiMinimalUserResponse> GetCodeTokenOwner(RequestContext context, 
        GameDatabaseContext database, string code)
    {
        DbCodeToken? token = database.GetCodeTokenWithCode(code);
        if (token == null)
            return ApiNotFoundError.TokenDoesNotExist;
        return ApiMinimalUserResponse.FromDb(token.User);
    }
    
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    [DocRequestBody(typeof(ApiSetEmailRequest))]
    [DocSummary("Set / change your account's e-mail address.")]
    [RateLimitSettings(300, 10, 300, "setEmail")]
    [Authentication(false)]
    [ApiEndpoint("setEmail", HttpMethods.Put)]
    public ApiOkResponse SetEmail(RequestContext context, GameDatabaseContext database, EmailService email, 
        ServerConfig config, ApiSetEmailRequest body)
    {
        DbCodeToken? token = database.GetCodeTokenWithCode(body.Code, CodeTokenType.SetEmail);
        if (token == null)
            return ApiUnauthorizedError.InvalidCode;

        if (!CommonPatterns.EmailAddressRegex().IsMatch(body.NewEmail))
            return ApiBadRequestError.InvalidEmail;

        string emailTemplate = @"
        <html>
            Hello {0},
            <br>

            Please use the verification code below to verify your email address.
            <br>

            <div style=""font-size: 36px; margin-top: 16px; margin-bottom: 16px;"">
            {1}
            </div>

            If you didn't request this, please ignore this email.

            Best regards, {2}.
        </html>
        ";


        DbCodeToken verifyEmailToken = database.CreateCodeToken(token.User, CodeTokenType.VerifyEmail);
        
        bool success = email.SendEmail(body.NewEmail, $"[{config.InstanceSettings.InstanceName}] Verify your Email Address",
            String.Format(emailTemplate, token.User.Name, verifyEmailToken.Code, config.InstanceSettings.InstanceName));

        if (!success)
        {
            return ApiInternalServerError.CouldNotSendEmail;
        }

        database.SetUserEmail(token.User, body.NewEmail);
        database.RemoveCodeToken(token);
        return new ApiOkResponse();
    }

    [DocSummary("Verify your account's email address.")]
    [DocRequestBody(typeof(ApiSetEmailRequest))]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [RateLimitSettings(300, 10, 300, "setEmail")]
    [Authentication(false)]
    [ApiEndpoint("verifyEmail", HttpMethods.Put)]
    public ApiOkResponse VerifyEmail(RequestContext context, GameDatabaseContext database, ApiCodeRequest body)
    {
        DbCodeToken? token = database.GetCodeTokenWithCode(body.Code, CodeTokenType.VerifyEmail);
        if (token == null)
            return ApiUnauthorizedError.InvalidCode;
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{token.User} successfully verified their email.");
        
        database.VerifyEmail(token.User);
        return new ApiOkResponse();
    }

    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    [DocRequestBody(typeof(ApiSendPasswordResetMailRequest))]
    [DocSummary("Emails you a code used to reset your password.")]
    [RateLimitSettings(300, 10, 300, "setPassword")]
    [Authentication(false)]
    [ApiEndpoint("sendPasswordResetMail", HttpMethods.Post)]
    public ApiOkResponse SendPasswordResetMail(RequestContext context, GameDatabaseContext database,
        EmailService email, ServerConfig config, ApiSendPasswordResetMailRequest body)
    {
        DbUser? user = database.GetUserWithEmail(body.Email);
        if (user == null)
        {
            // Don't respond with an error to avoid email lookup security vulnerability
            context.Logger.LogWarning(BunkumCategory.Authentication, 
                $"Tried sending password reset mail to a non existent user: \"{body.Email}\".");
            return new ApiOkResponse();
        }
        
        DbCodeToken codeToken = database.CreateCodeToken(user, CodeTokenType.SetPassword);
        
        string emailTemplate = @"
        <html>
            Hello {0},
            <br>

            Please use the code below to reset your password.
            <br>

            <div style=""font-size: 36px; margin-top: 16px; margin-bottom: 16px;"">
            {1}
            </div>

            If you didn't request this, please ignore this email.

            Best regards, {2}.
        </html>
        ";
        
        bool success = email.SendEmail(user.EmailAddress!, $"[{config.InstanceSettings.InstanceName}] Reset your password",
            String.Format(emailTemplate, codeToken.User.Name, codeToken.Code, config.InstanceSettings.InstanceName));

        return success ? new ApiOkResponse() : ApiInternalServerError.CouldNotSendEmail;
    }
    
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.PasswordIsNotHashedWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotBcryptPasswordWhen)]
    [DocSummary("Set a new password.")]
    [RateLimitSettings(300, 4, 300, "setPassword")]
    [Authentication(false)]
    [ApiEndpoint("setPassword", HttpMethods.Put)]
    public ApiOkResponse SetPassword(RequestContext context, GameDatabaseContext database, 
        ApiSetPasswordRequest body)
    {
        DbCodeToken? codeToken = database.GetCodeTokenWithCode(body.Code, CodeTokenType.SetPassword);
        if (codeToken == null)
            return ApiUnauthorizedError.InvalidCode;

        if (body.PasswordSha512.Length != 128 || !CommonPatterns.Sha512Regex().IsMatch(body.PasswordSha512))
            return ApiBadRequestError.PasswordIsNotHashed;

        string? passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor);
        if (passwordBcrypt == null) return ApiInternalServerError.CouldNotBcryptPassword;

        database.SetUserPassword(codeToken.User, passwordBcrypt);
        database.RemoveCodeToken(codeToken);
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{codeToken.User} successfully set their password.");

        return new ApiOkResponse();
    }
    
    
    // TODO: FINISH REGISTRATION. SHOULD BE A COMBINATION OF SET PASSWORD AND VERIFY EMAIL! SHOULD CHANGE TOKENTYPE FROM GAMEEULA TO GAMEACCESS 
}