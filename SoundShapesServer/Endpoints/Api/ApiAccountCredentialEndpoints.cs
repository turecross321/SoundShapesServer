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
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.ApiTypes.Errors;
using SoundShapesServer.Types.Responses.Api.DataTypes;

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
        DbCode? token = database.GetCode(body.Code, CodeType.SetEmail);
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


        DbCode verifyEmail = database.CreateCode(token.User, CodeType.VerifyEmail);
        
        bool success = email.SendEmail(body.NewEmail, $"[{config.InstanceSettings.InstanceName}] Verify your Email Address",
            String.Format(emailTemplate, token.User.Name, verifyEmail.Code, config.InstanceSettings.InstanceName));

        if (!success)
        {
            return ApiInternalServerError.CouldNotSendEmail;
        }

        database.SetUserEmail(token.User, body.NewEmail);
        database.RemoveCode(token);
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
        DbCode? code = database.GetCode(body.Code, CodeType.VerifyEmail);
        if (code == null)
            return ApiUnauthorizedError.InvalidCode;
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{code.User} successfully verified their email.");
        
        database.VerifyEmail(code.User);
        if (!code.User.FinishedRegistration)
            database.FinishUserRegistration(code.User);
        
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
        
        DbCode code = database.CreateCode(user, CodeType.SetPassword);
        
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
            String.Format(emailTemplate, code.User.Name, code.Code, config.InstanceSettings.InstanceName));

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
        DbCode? codeToken = database.GetCode(body.Code, CodeType.SetPassword);
        if (codeToken == null)
            return ApiUnauthorizedError.InvalidCode;

        if (body.PasswordSha512.Length != 128 || !CommonPatterns.Sha512Regex().IsMatch(body.PasswordSha512))
            return ApiBadRequestError.PasswordIsNotHashed;

        string? passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor);
        if (passwordBcrypt == null) return ApiInternalServerError.CouldNotBcryptPassword;

        database.SetUserPassword(codeToken.User, passwordBcrypt);
        database.RemoveCode(codeToken);
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{codeToken.User} successfully set their password.");

        return new ApiOkResponse();
    }
    
    [DocSummary("Retrieves information about the the specified registration code.")]
    [DocError(typeof(ApiNotFoundError), ApiUnauthorizedError.InvalidCodeWhen)]
    [ApiEndpoint("register/code/{codeValue}")]
    [Authentication(false)]
    public ApiResponse<ApiCodeResponse> GetRegistrationCode(RequestContext context, 
        GameDatabaseContext database, string codeValue)
    {
        DbCode? code = database.GetCode(codeValue, CodeType.Registration);
        if (code == null)
            return ApiUnauthorizedError.InvalidCode;
        return ApiCodeResponse.FromDb(code);
    }

    [DocSummary("Register an account. Assigns your e-mail and password and sends an e-mail verification mail.")]
    [DocRequestBody(typeof(ApiRegisterRequest))]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.EulaNotAcceptedWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotBcryptPasswordWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    [ApiEndpoint("register", HttpMethods.Post)]
    [Authentication(false)]
    public ApiOkResponse Register(RequestContext context, GameDatabaseContext database, ServerConfig config, 
        EmailService email, ApiRegisterRequest body)
    {
        DbCode? code = database.GetCode(body.Code, CodeType.Registration);
        
        if (code == null)
            return ApiUnauthorizedError.InvalidCode;
        if (!body.AcceptEula)
            return ApiUnauthorizedError.EulaNotAccepted;
        if (!CommonPatterns.EmailAddressRegex().IsMatch(body.Email))
            return ApiBadRequestError.InvalidEmail;
        
        string? passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.PasswordSha512, WorkFactor);
        if (passwordBcrypt == null) 
            return ApiInternalServerError.CouldNotBcryptPassword;
        
        string emailTemplate = @"
        <html>
            Hello {0},
            <br>

            Please use the verification code below to verify your email address and finish the registration of your account.
            <br>

            <div style=""font-size: 36px; margin-top: 16px; margin-bottom: 16px;"">
            {1}
            </div>

            If you didn't request this, please ignore this email.

            Best regards, {2}.
        </html>
        ";
        
        DbCode verifyEmail = database.CreateCode(code.User, CodeType.VerifyEmail);
        
        bool success = email.SendEmail(body.Email, $"[{config.InstanceSettings.InstanceName}] Verify your Email Address",
            String.Format(emailTemplate, code.User.Name, verifyEmail.Code, config.InstanceSettings.InstanceName));

        if (!success)
            return ApiInternalServerError.CouldNotSendEmail;

        database.SetUserEmail(code.User, body.Email);
        database.SetUserPassword(code.User, passwordBcrypt);
        database.RemoveCode(code);

        return new ApiOkResponse();
    }
}