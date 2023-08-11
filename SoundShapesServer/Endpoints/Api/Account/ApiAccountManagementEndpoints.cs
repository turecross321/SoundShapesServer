using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using AttribDoc.Attributes;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
using SoundShapesServer.Documentation.Errors;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
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
    [DocError(typeof(BadRequestError), BadRequestError.InvalidUsernameWhen)]
    [DocError(typeof(ConflictError), ConflictError.UsernameAlreadyTakenWhen)]
    public Response SetUsername(RequestContext context, GameDatabaseContext database, GameUser user, ApiSetUsernameRequest body)
    {
        if (!UserHelper.IsUsernameLegal(body.NewUsername)) return new Response(BadRequestError.InvalidUsernameWhen, ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        GameUser? userWithRequestedName = database.GetUserWithUsername(body.NewUsername);
        if (userWithRequestedName != null)
            return new Response(ConflictError.UsernameAlreadyTakenWhen, ContentType.Plaintext, HttpStatusCode.Conflict);
        
        database.SetUsername(user, body.NewUsername);
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendEmailSession", Method.Post)]
    [DocSummary("Sends an email containing a session ID that is required to change your email address.")]
    [DocError(typeof(InternalServerError), InternalServerError.CouldNotSendEmailWhen)]
    public Response SendEmailSession(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        string emailSessionId = GenerateEmailSessionId(database);
        GameSession emailSession = database.CreateSession(user, SessionType.SetEmail, PlatformType.Api, Globals.TenMinutesInSeconds, emailSessionId);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";

        try
        {
            emailService.SendEmail(user.Email!, "Sound Shapes New Email Code", emailBody);
            return HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            return new Response(InternalServerError.CouldNotSendEmailWhen, ContentType.Plaintext, HttpStatusCode.InternalServerError);
        }
    }
    
    [ApiEndpoint("account/setEmail", Method.Post)]
    [DocSummary("Changes your email address.")]
    [DocError(typeof(BadRequestError), BadRequestError.InvalidEmailWhen)]
    [DocError(typeof(ConflictError), ConflictError.EmailAlreadyTakenWhen)]
    public Response SetUserEmail(RequestContext context, GameDatabaseContext database, ApiSetEmailRequest body, GameSession session, EmailService emailService)
    {
        GameUser user = session.User;

        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
            return new Response(BadRequestError.InvalidEmailWhen, ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null && userWithEmail.Id != user.Id) 
            return new Response(ConflictError.EmailAlreadyTakenWhen, ContentType.Plaintext, HttpStatusCode.Conflict);

        database.SetUserEmail(user, body.NewEmail);
        database.RemoveSession(session);
        
        if (!user.HasFinishedRegistration)
            return SendPasswordSession(context, database, new ApiPasswordSessionRequest {Email = body.NewEmail}, emailService);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendPasswordSession", Method.Post), Authentication(false)]
    [DocSummary("Sends an email containing a session ID that is required to change your password.")]
    [DocError(typeof(InternalServerError), InternalServerError.CouldNotSendEmailWhen)]
    public Response SendPasswordSession(RequestContext context, GameDatabaseContext database, ApiPasswordSessionRequest body, EmailService emailService)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return HttpStatusCode.Created;

        string passwordSessionId = GeneratePasswordSessionId(database);
        GameSession passwordSession = database.CreateSession(user, SessionType.SetPassword, PlatformType.Api, Globals.TenMinutesInSeconds, passwordSessionId);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordSession.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";
        
        try
        {
            emailService.SendEmail(user.Email!, "Sound Shapes Password Code", emailBody);
            return HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            return new Response(InternalServerError.CouldNotSendEmailWhen, ContentType.Plaintext, HttpStatusCode.InternalServerError);
        }
    }
    
    [ApiEndpoint("account/setPassword", Method.Post)]
    [DocSummary("Changes your password.")]
    [DocError(typeof(BadRequestError), BadRequestError.PasswordIsNotHashedWhen)]
    public Response SetUserPassword(RequestContext context, GameDatabaseContext database, ApiSetPasswordRequest body, GameSession session)
    {
        GameUser user = session.User;

        if (!Sha512Regex().IsMatch(body.NewPasswordSha512))
            return new Response(BadRequestError.PasswordIsNotHashedWhen,
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        database.SetUserPassword(user, body.NewPasswordSha512);
        database.RemoveSession(session);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendRemovalSession", Method.Post)]
    [DocSummary("Sends an email containing a session ID that is required to delete your account.")]
    [DocError(typeof(InternalServerError), InternalServerError.CouldNotSendEmailWhen)]
    public Response SendUserRemovalSession(RequestContext context, GameDatabaseContext database, GameUser user, GameSession session, EmailService emailService)
    {
        string removalSessionId = GenerateAccountRemovalSessionId(database);
        GameSession removalSession = database.CreateSession(user, SessionType.RemoveAccount, PlatformType.Api, Globals.TenMinutesInSeconds, removalSessionId);

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        try
        {
            emailService.SendEmail(user.Email!, "Sound Shapes Account Removal Code", emailBody);
            return HttpStatusCode.Created;
        }
        catch (Exception e)
        {
            return new Response(InternalServerError.CouldNotSendEmailWhen, ContentType.Plaintext, HttpStatusCode.InternalServerError);
        }
    }

    [ApiEndpoint("account", Method.Delete)]
    [DocSummary("Deletes your account.")]
    public Response RemoveAccount(RequestContext context, GameDatabaseContext database, GameUser user, IDataStore dataStore)
    {
        database.RemoveUser(user, dataStore);
        return HttpStatusCode.NoContent;
    }
}