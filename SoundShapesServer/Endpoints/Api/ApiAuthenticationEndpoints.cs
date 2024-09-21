using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses;
using Bunkum.Listener.Protocol;
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
using static BCrypt.Net.BCrypt;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAuthenticationEndpoints : EndpointGroup
{
    /// <remarks>
    ///     If increased, passwords will automatically be rehashed at login time to use the new WorkFactor 
    ///     If decreased, passwords will stay at higher WorkFactor until reset
    /// </remarks>
    public const int WorkFactor = 14;
    
    /// <summary>
    /// A randomly generated password.
    /// Used to prevent against timing attacks.
    /// </summary>
    private static readonly string FakePassword = HashPassword(Random.Shared.Next().ToString(), WorkFactor);
    
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    [DocRequestBody(typeof(ApiSetEmailRequest))]
    [DocSummary("Set your account's e-mail address.")]
    [RateLimitSettings(300, 10, 300, "setEmail")]
    [ApiEndpoint("setEmail", HttpMethods.Post)]
    public ApiOkResponse SetEmail(RequestContext context, GameDatabaseContext database, EmailService email, 
        ServerConfig config, BunkumConfig bunkumConfig, DbUser user, ApiSetEmailRequest body)
    {
        if (!CommonPatterns.EmailAddressRegex().IsMatch(body.NewEmail))
            return ApiBadRequestError.InvalidEmail;


        DbCode verifyEmail = database.CreateCode(user, CodeType.VerifyEmail);
        string verifyUrl = bunkumConfig.ExternalUrl + ApiEndpointAttribute.RoutePrefix + "/verifyEmail/code/" +
                           verifyEmail.Code;
        
        string htmlBody = @"
        <html style='background-color: #2F2A2A; margin: 0;' lang='en'>
        <head>
            <style>
                @import url('https://fonts.googleapis.com/css2?family=Sora:wght@400;700&display=swap');
                html { font-family: Sora, Rubik, system-ui, sans-serif; }
                p { margin: 0; }
                body { margin: 0; padding-left: 32px; padding-right: 32px; font-size: medium; 
                    filter: drop-shadow(0 1px 3px rgba(0, 0, 0, 0.1)) drop-shadow(0 1px 2px rgba(0, 0, 0, 0.06)); }
                main { max-width: 600px; margin: 2rem auto 0; background-color: #FFF0E4; padding: 32px; 
                    border-radius: 1rem; min-height: 480px; display: flex; flex-direction: column; justify-content: space-between; }
                #code { color: white; background-color: #F07167; padding: 1rem 2rem; border-radius: 1rem; font-size: x-large; 
                    border: none; text-decoration: none; width: min-content; transition: background-color 150ms ease, box-shadow 150ms ease; }
                #code:hover { background-color: rgb(216, 102, 93); filter: drop-shadow(0 1px 3px rgba(0, 0, 0, 0.1)) drop-shadow(0 1px 2px rgba(0, 0, 0, 0.06)); }
            </style>
            <title>Verification</title>
        </head>
        <body>
            <main>
                <div style='display: flex; flex-direction: column; gap: 0.5rem;'>
                    <p style='font-size: x-large; font-weight: bold;'>Hello {USER},</p>
                    <p>Please click the button below to verify your email address. Greetings, the {INSTANCE} team.</p>
                    <a id='code' href='{CODE_URL}'>Verify</a>
                </div>
                <p>If you didn't request this, please ignore this email.</p>
            </main>
        </body>
        </html>";

        // Replace placeholders in the HTML template
        htmlBody = htmlBody.Replace("{USER}", user.Name)
            .Replace("{INSTANCE}", config.InstanceSettings.InstanceName)
            .Replace("{CODE_URL}", verifyUrl);
        
        bool success = email.SendEmail(body.NewEmail,
                $"[{config.InstanceSettings.InstanceName}] Verify your Email Address", htmlBody);

        if (!success)
        {
            return ApiInternalServerError.CouldNotSendEmail;
        }

        database.SetUserEmail(user, body.NewEmail);
        return new ApiOkResponse();
    }

    [DocSummary("Verify your account's email address.")] 
    [DocError(typeof(string), ApiUnauthorizedError.InvalidCodeWhen)]
    [RateLimitSettings(300, 10, 300, "setEmail")]
    [Authentication(false)]
    [ApiEndpoint("verifyEmail/code/{codeValue}")]
    public Response VerifyEmail(RequestContext context, GameDatabaseContext database, ServerConfig config, string codeValue)
    {
        DbCode? code = database.GetCode(codeValue, CodeType.VerifyEmail);
        if (code == null)
            return new Response(ApiUnauthorizedError.InvalidCodeWhen, ContentType.Plaintext, Unauthorized);
        
        DbUser user = database.VerifyEmail(code.User);
        if (!user.FinishedRegistration)
        {
            user = database.FinishUserRegistration(code.User);
            context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} successfully finish their registration.");
        }
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} successfully verified their email.");
        context.ResponseHeaders.Add("Location", config.WebsiteUrl);
        
        return new Response(SeeOther);
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
        
        string htmlBody = @"
        <html style='background-color: #2F2A2A; margin: 0;' lang='en'>
        <head>
            <style>
                @import url('https://fonts.googleapis.com/css2?family=Sora:wght@400;700&display=swap');
                html { font-family: Sora, Rubik, system-ui, sans-serif; }
                p { margin: 0; }
                body { margin: 0; padding-left: 32px; padding-right: 32px; font-size: medium;
                    filter: drop-shadow(0 1px 3px rgba(0, 0, 0, 0.1)) drop-shadow(0 1px 2px rgba(0, 0, 0, 0.06)); }
                main { max-width: 600px; margin: 2rem auto 0; background-color: #FFF0E4; padding: 32px;
                    border-radius: 1rem; min-height: 480px; display: flex; flex-direction: column; justify-content: space-between; }
                #code { color: white; background-color: #F07167; padding: 1rem 2rem; border-radius: 1rem; font-size: x-large;
                    border: none; text-decoration: none; width: min-content; transition: background-color 150ms ease, box-shadow 150ms ease; }
            </style>
            <title>Verification</title>
        </head>
        <body>
            <main>
                <div style='display: flex; flex-direction: column; gap: 0.5rem;'>
                    <p style='font-size: x-large; font-weight: bold;'>Hello {USER},</p>
                    <p>You may use the code below to reset your password. Greetings, the {INSTANCE} team.</p>
                    <p id='code'>{CODE}</p>
                </div>
                <p>If you didn't request this, please ignore this email.</p>
            </main>
        </body>
        </html>
        ";
        
        // Replace placeholders in the HTML template
        htmlBody = htmlBody.Replace("{USER}", user.Name)
            .Replace("{INSTANCE}", config.InstanceSettings.InstanceName)
            .Replace("{CODE}", code.Code);
        
        bool success = email.SendEmail(user.EmailAddress!,
            $"[{config.InstanceSettings.InstanceName}] Reset your password", htmlBody);

        return success ? new ApiOkResponse() : ApiInternalServerError.CouldNotSendEmail;
    }
    
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidCodeWhen)]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.PasswordIsNotHashedWhen)]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotBcryptPasswordWhen)]
    [DocSummary("Set a new password.")]
    [RateLimitSettings(300, 4, 300, "setPassword")]
    [Authentication(false)]
    [ApiEndpoint("resetPassword", HttpMethods.Post)]
    public ApiOkResponse ResetPassword(RequestContext context, GameDatabaseContext database, 
        ApiSetPasswordRequest body)
    {
        DbCode? codeToken = database.GetCode(body.Code, CodeType.SetPassword);
        if (codeToken == null)
            return ApiUnauthorizedError.InvalidCode;

        if (body.PasswordSha512.Length != 128 || !CommonPatterns.Sha512Regex().IsMatch(body.PasswordSha512))
            return ApiBadRequestError.PasswordIsNotHashed;

        string? passwordBcrypt = HashPassword(body.PasswordSha512, WorkFactor);
        if (passwordBcrypt == null) return ApiInternalServerError.CouldNotBcryptPassword;

        database.SetUserPassword(codeToken.User, passwordBcrypt);
        database.RemoveCode(codeToken);
        
        context.Logger.LogInfo(BunkumCategory.Authentication, $"{codeToken.User} successfully set their password.");

        return new ApiOkResponse();
    }
    
    [DocSummary("Retrieves information about the the specified registration code.")]
    [DocResponseBody(typeof(ApiCodeResponse))]    
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
        BunkumConfig bunkumConfig, EmailService email, ApiRegisterRequest body)
    {
        DbCode? code = database.GetCode(body.Code, CodeType.Registration);
        
        if (code == null)
            return ApiUnauthorizedError.InvalidCode;
        if (!body.AcceptEula)
            return ApiUnauthorizedError.EulaNotAccepted;
        if (!CommonPatterns.EmailAddressRegex().IsMatch(body.Email))
            return ApiBadRequestError.InvalidEmail;
        
        string? passwordBcrypt = HashPassword(body.PasswordSha512, WorkFactor);
        if (passwordBcrypt == null) 
            return ApiInternalServerError.CouldNotBcryptPassword;
        
        DbCode verifyEmail = database.CreateCode(code.User, CodeType.VerifyEmail);
        string verifyUrl = bunkumConfig.ExternalUrl + ApiEndpointAttribute.RoutePrefix + "/verifyEmail/code/" +
                           verifyEmail.Code;
        
        string htmlBody = @"
        <html style='background-color: #2F2A2A; margin: 0;' lang='en'>
        <head>
            <style>
                @import url('https://fonts.googleapis.com/css2?family=Sora:wght@400;700&display=swap');
                html { font-family: Sora, Rubik, system-ui, sans-serif; }
                p { margin: 0; }
                body { margin: 0; padding-left: 32px; padding-right: 32px; font-size: medium;
                    filter: drop-shadow(0 1px 3px rgba(0, 0, 0, 0.1)) drop-shadow(0 1px 2px rgba(0, 0, 0, 0.06)); }
                main { max-width: 600px; margin: 2rem auto 0; background-color: #FFF0E4; padding: 32px;
                    border-radius: 1rem; min-height: 480px; display: flex; flex-direction: column; justify-content: space-between; }
                #code { color: white; background-color: #F07167; padding: 1rem 2rem; border-radius: 1rem; font-size: x-large;
                    border: none; text-decoration: none; width: min-content; transition: background-color 150ms ease, box-shadow 150ms ease; }
                #code:hover { background-color: rgb(216, 102, 93); filter: drop-shadow(0 1px 3px rgba(0, 0, 0, 0.1)) drop-shadow(0 1px 2px rgba(0, 0, 0, 0.06)); }
            </style>
            <title>Verification</title>
        </head>
        <body>
            <main>
                <div style='display: flex; flex-direction: column; gap: 0.5rem;'>
                    <p style='font-size: x-large; font-weight: bold;'>Hello {USER},</p>
                    <p>Please click the button below to verify your email address and finish the registration of your account. Greetings, the {INSTANCE} team.</p>
                    <a id='code' href='{CODE_URL}'>Verify</a>
                </div>
                <p>If you didn't request this, please ignore this email.</p>
            </main>
        </body>
        </html>";

        // Replace placeholders in the HTML template
        htmlBody = htmlBody.Replace("{USER}", code.User.Name)
            .Replace("{INSTANCE}", config.InstanceSettings.InstanceName)
            .Replace("{CODE_URL}", verifyUrl);
        
        bool success = email.SendEmail(body.Email,
            $"[{config.InstanceSettings.InstanceName}] Verify your Email Address", htmlBody);

        if (!success)
            return ApiInternalServerError.CouldNotSendEmail;

        database.SetUserEmail(code.User, body.Email);
        database.SetUserPassword(code.User, passwordBcrypt);
        database.RemoveCode(code);

        return new ApiOkResponse();
    }
    
    [DocResponseBody(typeof(ApiLoginResponse))]
    [DocRequestBody(typeof(ApiLogInRequest))]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.InvalidEmailOrPasswordWhen)]
    [RateLimitSettings(300, 10, 300, "auth")]
    [Authentication(false)]
    [ApiEndpoint("logIn", HttpMethods.Post)]
    public ApiResponse<ApiLoginResponse> LogIn(RequestContext context, GameDatabaseContext database, ApiLogInRequest body)
    {
        DbUser? user = database.GetUserWithEmail(body.Email);
        if (user == null)
        {
            // Do the work of checking the password if there was no user found to avoid timing attacks.
            _ = Verify(body.PasswordSha512, FakePassword);

            return ApiUnauthorizedError.InvalidEmailOrPassword;
        }

        if (!Verify(body.PasswordSha512, user.PasswordBcrypt))
            return ApiUnauthorizedError.InvalidEmailOrPassword;
        
        if (PasswordNeedsRehash(user.PasswordBcrypt, WorkFactor))
            database.SetUserPassword(user, HashPassword(body.PasswordSha512, WorkFactor));

        DbRefreshToken refreshToken = database.CreateRefreshToken(user);
        DbToken token = database.CreateToken(user, TokenType.ApiAccess, null, null, refreshToken, null);

        context.Logger.LogInfo(BunkumCategory.Authentication, $"{user} successfully logged in through the API");

        return new ApiLoginResponse
        {
            User = ApiFullUserResponse.FromDb(user),
            AccessToken = ApiTokenResponse.FromDb(token),
            RefreshToken = ApiRefreshTokenResponse.FromDb(refreshToken)
        };
    }

    [DocSummary("Log in with a refresh token.")]
    [DocResponseBody(typeof(ApiLoginResponse))]
    [DocRequestBody(typeof(ApiRefreshTokenRequest))]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.RefreshTokenDoesNotExistWhen)]
    [RateLimitSettings(300, 10, 300, "auth")]
    [Authentication(false)]
    [ApiEndpoint("refreshToken", HttpMethods.Post)]
    public ApiResponse<ApiLoginResponse> LogInWithRefreshToken(RequestContext context, GameDatabaseContext database, 
        ApiRefreshTokenRequest body)
    {
        DbRefreshToken? refreshToken = database.GetRefreshTokenWithId(body.RefreshTokenId);
        if (refreshToken == null)
            return ApiNotFoundError.RefreshTokenDoesNotExist;

        DbToken token = database.CreateApiTokenWithRefreshToken(refreshToken);

        return new ApiLoginResponse
        {
            User = ApiFullUserResponse.FromDb(refreshToken.User),
            AccessToken = ApiTokenResponse.FromDb(token),
            RefreshToken = ApiRefreshTokenResponse.FromDb(refreshToken)
        };
    }

    [DocSummary("Revoke your access token and its associated refresh token with all its other tokens.")]
    [ApiEndpoint("revokeToken", HttpMethods.Post)]
    public ApiOkResponse LogOut(RequestContext context, GameDatabaseContext database, DbToken token)
    {
        database.RemoveToken(token);
        return new ApiOkResponse();
    }
}