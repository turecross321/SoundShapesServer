using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Services;
using SoundShapesServer.Types;
using static SoundShapesServer.Helpers.SessionHelper;

namespace SoundShapesServer.Endpoints.Api;

public class ApiAccountSettingEndpoints : EndpointGroup
{
    private const string Sha512Pattern = "^[a-f0-9]{128}$";

    [ApiEndpoint("account/setUsername", Method.Post)]
    public Response SetUsername(RequestContext context, RealmDatabaseContext database, GameUser user, ApiSetUsernameRequest body)
    {
        if (!UserHelper.IsUsernameLegal(body.NewUsername)) return new Response("Not a valid username.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        GameUser? userWithNewName = database.GetUserWithUsername(body.NewUsername);
        if (userWithNewName != null)
        {
            return new Response("Username is not available.", ContentType.Plaintext, HttpStatusCode.Conflict);
        }
        
        database.SetUsername(user, body.NewUsername);
        
        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendEmailSession", Method.Post)]
    public Response SendEmailSession(RequestContext context, RealmDatabaseContext database, GameUser user, ApiEmailSessionRequest body)
    {
        string emailSessionId = GenerateEmailSessionId(database);
        GameSession emailSession = database.GenerateSessionForUser(context, user, SessionType.SetEmail, 600, emailSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";

        EmailService emailService = context.Services.OfType<EmailService>().First();
        emailService.SendEmail(body.NewEmail, "Sound Shapes New Email Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setEmail", Method.Post)]
    public Response SetUserEmail(RequestContext context, RealmDatabaseContext database, ApiSetEmailRequest body, GameSession session)
    {
        if (session.User == null) return HttpStatusCode.NotFound;
        GameUser user = session.User;

        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
        {
            return new Response("Invalid Email.", ContentType.Json, HttpStatusCode.BadRequest);
        }
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null && userWithEmail.Id != user.Id) return new Response("Email is already in use.", ContentType.Json, HttpStatusCode.Forbidden);

        database.SetUserEmail(user, body.NewEmail, session);
        
        if (!user.HasFinishedRegistration)
            return SendPasswordSession(context, database, new ApiPasswordSessionRequest(body.NewEmail));

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendPasswordSession", Method.Post)]
    [Authentication(false)]
    public Response SendPasswordSession(RequestContext context, RealmDatabaseContext database, ApiPasswordSessionRequest body)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return HttpStatusCode.Created; // trol

        string passwordSessionId = GeneratePasswordSessionId(database);
        GameSession passwordSession = database.GenerateSessionForUser(context, user, SessionType.SetPassword, 600, passwordSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordSession.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";

        EmailService emailService = context.Services.OfType<EmailService>().First();
        emailService.SendEmail(body.Email, "Sound Shapes Password Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setPassword", Method.Post)]
    public Response SetUserPassword(RequestContext context, RealmDatabaseContext database, ApiSetPasswordRequest body, GameSession session)
    {
        if (session.User == null) return HttpStatusCode.Gone;
        GameUser user = session.User;

        if (body.NewPasswordSha512.Length != 128 || !Regex.IsMatch(body.NewPasswordSha512, Sha512Pattern))
            return new Response("Password is definitely not SHA512. Please hash the password.",
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        string passwordBcrypt = BCrypt.Net.BCrypt.HashPassword(body.NewPasswordSha512, ApiAuthenticationEndpoints.WorkFactor);

        return database.SetUserPassword(user, passwordBcrypt, session) ? HttpStatusCode.Created : HttpStatusCode.InternalServerError;
    }

    [ApiEndpoint("account/sendRemovalSession", Method.Post)]
    public Response SendUserRemovalSession(RequestContext context, RealmDatabaseContext database, GameUser user, GameSession session)
    {
        if (user.Email == null)
            return new Response(
                "User does not have a linked email. Please contact a system administrator to remove your account.",
                ContentType.Plaintext, HttpStatusCode.Forbidden);
        
        string removalSessionId = GenerateAccountRemovalSessionId(database);
        GameSession removalSession = database.GenerateSessionForUser(context, user, SessionType.RemoveAccount, 600, removalSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";

        EmailService emailService = context.Services.OfType<EmailService>().First();
        emailService.SendEmail(user.Email, "Sound Shapes Account Removal Code", emailBody);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/remove", Method.Post)]
    public Response RemoveAccount(RequestContext context, RealmDatabaseContext database, GameUser user, IDataStore dataStore)
    {
        database.RemoveUser(user, dataStore);
        return new Response("o7", ContentType.Plaintext);
    }
}