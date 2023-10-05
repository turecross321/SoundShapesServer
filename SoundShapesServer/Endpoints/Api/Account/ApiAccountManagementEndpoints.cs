using System.Net.Mail;
using System.Text.RegularExpressions;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Attributes;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Framework.Errors;
using SoundShapesServer.Services;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Endpoints.Api.Account;

public partial class ApiAccountManagementEndpoints : EndpointGroup
{
    [GeneratedRegex("^[a-fA-F0-9]{128}$")]
    private static partial Regex Sha512Regex();
    
    [ApiEndpoint("account/setUsername", Method.Post)]
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

    [ApiEndpoint("account/sendEmailSession", Method.Post)]
    [MinimumPermissions(PermissionsType.Banned)]
    [DocSummary("Sends an email containing a session ID that is required to change your email address.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendEmailSession(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        GameSession emailSession = database.CreateSession(user, SessionType.SetEmail, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";

        if (emailService.SendEmail(user.Email!, "Sound Shapes New Email Code", emailBody))
            return new ApiOkResponse();
        
        return ApiInternalServerError.CouldNotSendEmail;
    }
    
    [ApiEndpoint("account/setEmail", Method.Post), Authentication(false)]
    [DocSummary("Changes your email address.")]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ApiConflictError), ApiConflictError.EmailAlreadyTakenWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.SessionDoesNotExistWhen)]
    public ApiOkResponse SetUserEmail(RequestContext context, GameDatabaseContext database, EmailService emailService, ApiSetEmailRequest body)
    {
        GameSession? session = database.GetSessionWithId(body.SetEmailSessionId);
        if (session == null)
            return ApiNotFoundError.SessionDoesNotExist;
        
        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
            return ApiBadRequestError.InvalidEmail;
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null) 
            return ApiConflictError.EmailAlreadyTaken;


        
        database.SetUserEmail(session.User, body.NewEmail);
        database.RemoveSession(session);
        
        if (!session.User.HasFinishedRegistration)
            return SendPasswordSession(context, database, new ApiPasswordSessionRequest {Email = body.NewEmail}, emailService);

        return new ApiOkResponse();
    }

    [ApiEndpoint("account/sendPasswordSession", Method.Post), Authentication(false)]
    [DocSummary("Sends an email containing a session ID that is required to change your password.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendPasswordSession(RequestContext context, GameDatabaseContext database, ApiPasswordSessionRequest body, EmailService emailService)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) 
            return new ApiOkResponse();
        
        GameSession passwordSession = database.CreateSession(user, SessionType.SetPassword, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordSession.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";

        if (emailService.SendEmail(user.Email!, "Sound Shapes Password Code", emailBody))
            return new ApiOkResponse();
        
        return ApiInternalServerError.CouldNotSendEmail;
    }
    
    [ApiEndpoint("account/setPassword", Method.Post), Authentication(false)]
    [DocSummary("Changes your password.")]
    [DocError(typeof(ApiBadRequestError), ApiBadRequestError.PasswordIsNotHashedWhen)]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.SessionDoesNotExistWhen)]
    public ApiOkResponse SetUserPassword(RequestContext context, GameDatabaseContext database, ApiSetPasswordRequest body)
    {
        if (!Sha512Regex().IsMatch(body.NewPasswordSha512))
            return ApiBadRequestError.PasswordIsNotHashed;

        GameSession? session = database.GetSessionWithId(body.SetPasswordSessionId);
        if (session == null)
            return ApiNotFoundError.SessionDoesNotExist;
        
        database.SetUserPassword(session.User, body.NewPasswordSha512);
        // All sessions are wiped automatically by GameDatabaseContext

        return new ApiOkResponse();
    }

    [ApiEndpoint("account/sendRemovalSession", Method.Post)]
    [MinimumPermissions(PermissionsType.Banned)]
    [DocSummary("Sends an email containing a session ID that is required to delete your account.")]
    [DocError(typeof(ApiInternalServerError), ApiInternalServerError.CouldNotSendEmailWhen)]
    public ApiOkResponse SendUserRemovalSession(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        GenerateAccountRemovalSessionId(database);
        GameSession removalSession = database.CreateSession(user, SessionType.AccountRemoval, Globals.TenMinutesInSeconds);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        
        if (emailService.SendEmail(user.Email!, "Sound Shapes Account Removal Code", emailBody))
            return new ApiOkResponse();
            
        return ApiInternalServerError.CouldNotSendEmail;
    }

    [ApiEndpoint("account", Method.Delete), Authentication(false)]
    [DocSummary("Deletes your account.")]
    [DocError(typeof(ApiNotFoundError), ApiNotFoundError.SessionDoesNotExistWhen)]
    public ApiOkResponse RemoveAccount(RequestContext context, GameDatabaseContext database, IDataStore dataStore, ApiRemoveAccountRequest body)
    {
        GameSession? session = database.GetSessionWithId(body.AccountRemovalSessionId);
        if (session == null)
            return ApiNotFoundError.SessionDoesNotExist;
        
        database.RemoveUser(session.User, dataStore);
        return new ApiOkResponse();
    }
}