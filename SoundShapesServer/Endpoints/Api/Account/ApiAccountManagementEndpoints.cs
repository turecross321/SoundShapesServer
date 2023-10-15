using System.Net.Mail;
using System.Text.RegularExpressions;
using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Services;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Endpoints.Api.Account;

public partial class ApiAccountManagementEndpoints : EndpointGroup
{
    [GeneratedRegex("^[a-fA-F0-9]{128}$")]
    private static partial Regex Sha512Regex();
    
    [ApiEndpoint("account/setUsername", HttpMethods.Post)]
    [DocSummary("Changes your username.")]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidUsernameWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.UsernameAlreadyTakenWhen)]
    public ApiOkResponse SetUsername(RequestContext context, GameDatabaseContext database, GameUser user, ApiSetUsernameRequest body)
    {
        if (!UserHelper.IsUsernameLegal(body.NewUsername)) 
            return ApiBadRequestError.InvalidUsername;
        
        GameUser? userWithRequestedName = database.GetUserWithUsername(body.NewUsername);
        if (userWithRequestedName != null)
            return ApiConflictError.UsernameAlreadyTaken;
        
        database.SetUsername(user, body.NewUsername);
        return new ApiOkResponse();
    }

    [ApiEndpoint("account/register", HttpMethods.Post), Authentication(false)]
    [DocSummary("Used to create an account.")]
    [DocError(typeof(ApiUnauthorizedError), ApiUnauthorizedError.EulaNotAcceptedWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.RegistrationTokenDoesNotExistWhen)]
    public ApiOkResponse RegisterAccount(RequestContext context, GameDatabaseContext database, ApiRegisterAccountRequest body)
    {
        if (!body.AcceptEula)
            return ApiUnauthorizedError.EulaNotAccepted;
        
        GameToken? registrationToken = database.GetTokenWithId(body.RegistrationCode, TokenType.AccountRegistration);
        if (registrationToken == null)
            return ApiNotFoundError.RegistrationTokenDoesNotExist;

        GameUser user = registrationToken.User;
        database.SetUserEmail(user, body.Email);
        database.SetUserPassword(user, body.PasswordSha512);
        database.FinishRegistration(user);

        return new ApiOkResponse();
    }

    [ApiEndpoint("account/sendEmailToken", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Banned)]
    [DocSummary("Sends an email containing a token ID that is required to change your email address.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendEmailToken(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        GameToken emailToken = database.CreateToken(user, TokenType.SetEmail, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailToken.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";

        if (emailService.SendEmail(user.Email!, "Sound Shapes New Email Code", emailBody))
            return new ApiOkResponse();
        
        return ApiInternalServerError.CouldNotSendEmail;
    }
    
    [ApiEndpoint("account/setEmail", HttpMethods.Post), Authentication(false)]
    [DocSummary("Changes your email address.")]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.EmailAlreadyTakenWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.TokenDoesNotExistWhen)]
    public ApiOkResponse SetUserEmail(RequestContext context, GameDatabaseContext database, EmailService emailService, ApiSetEmailRequest body)
    {
        GameToken? token = database.GetTokenWithId(body.SetEmailTokenId, TokenType.SetEmail);
        if (token == null)
            return ApiNotFoundError.TokenDoesNotExist;
        
        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
            return ApiBadRequestError.InvalidEmail;
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null) 
            return ApiConflictError.EmailAlreadyTaken;
        
        database.SetUserEmail(token.User, body.NewEmail);

        // todo: email verification

        database.RemoveToken(token);
        return new ApiOkResponse();
    }

    [ApiEndpoint("account/sendPasswordToken", HttpMethods.Post), Authentication(false)]
    [DocSummary("Sends an email containing a token ID that is required to change your password.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendPasswordToken(RequestContext context, GameDatabaseContext database, ApiPasswordTokenRequest body, EmailService emailService)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) 
            return new ApiOkResponse();
        
        GameToken passwordToken = database.CreateToken(user, TokenType.SetPassword, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordToken.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";

        if (emailService.SendEmail(user.Email!, "Sound Shapes Password Code", emailBody))
            return new ApiOkResponse();
        
        return ApiInternalServerError.CouldNotSendEmail;
    }
    
    [ApiEndpoint("account/setPassword", HttpMethods.Post), Authentication(false)]
    [DocSummary("Changes your password.")]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.PasswordIsNotHashedWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.TokenDoesNotExistWhen)]
    public ApiOkResponse SetUserPassword(RequestContext context, GameDatabaseContext database, ApiSetPasswordRequest body)
    {
        if (!Sha512Regex().IsMatch(body.NewPasswordSha512))
            return ApiBadRequestError.PasswordIsNotHashed;

        GameToken? token = database.GetTokenWithId(body.SetPasswordTokenId, TokenType.SetPassword);
        if (token == null)
            return ApiNotFoundError.TokenDoesNotExist;
        
        database.SetUserPassword(token.User, body.NewPasswordSha512);

        return new ApiOkResponse();
    }

    [ApiEndpoint("account/sendRemovalToken", HttpMethods.Post)]
    [MinimumPermissions(PermissionsType.Banned)]
    [DocSummary("Sends an email containing a token ID that is required to delete your account.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendUserRemovalToken(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        GameToken removalToken = database.CreateToken(user, TokenType.AccountRemoval, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalToken.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        
        if (emailService.SendEmail(user.Email!, "Sound Shapes Account Removal Code", emailBody))
            return new ApiOkResponse();
            
        return ApiInternalServerError.CouldNotSendEmail;
    }

    [ApiEndpoint("account", HttpMethods.Delete), Authentication(false)]
    [DocSummary("Deletes your account.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.TokenDoesNotExistWhen)]
    public ApiOkResponse RemoveAccount(RequestContext context, GameDatabaseContext database, IDataStore dataStore, ApiRemoveAccountRequest body)
    {
        GameToken? token = database.GetTokenWithId(body.AccountRemovalTokenId, TokenType.AccountRemoval);
        if (token == null)
            return ApiNotFoundError.TokenDoesNotExist;
        
        database.RemoveUser(token.User, dataStore);
        return new ApiOkResponse();
    }
}