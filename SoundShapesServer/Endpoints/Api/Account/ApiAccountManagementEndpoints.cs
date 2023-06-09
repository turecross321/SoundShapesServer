using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Database;
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
    public Response SetUsername(RequestContext context, GameDatabaseContext database, GameUser user, ApiSetUsernameRequest body)
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
    public Response SendEmailSession(RequestContext context, GameDatabaseContext database, GameUser user, EmailService emailService)
    {
        string emailSessionId = GenerateEmailSessionId(database);
        GameSession emailSession = database.CreateSession(user, SessionType.SetEmail, PlatformType.Api, 600, emailSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your new email code: " + emailSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        
        emailService.SendEmail(user.Email!, "Sound Shapes New Email Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setEmail", Method.Post)]
    public Response SetUserEmail(RequestContext context, GameDatabaseContext database, ApiSetEmailRequest body, GameSession session, EmailService emailService)
    {
        GameUser user = session.User;

        // Check if user has sent a valid mail address
        if (MailAddress.TryCreate(body.NewEmail, out MailAddress? _) == false)
            return new Response("Invalid Email.", ContentType.Plaintext, HttpStatusCode.BadRequest);
        
        // Check if mail address has been used before
        GameUser? userWithEmail = database.GetUserWithEmail(body.NewEmail);
        if (userWithEmail != null && userWithEmail.Id != user.Id) 
            return new Response("Email is already in use.", ContentType.Plaintext, HttpStatusCode.Forbidden);

        database.SetUserEmail(user, body.NewEmail);
        database.RemoveSession(session);
        
        if (!user.HasFinishedRegistration)
            return SendPasswordSession(context, database, new ApiPasswordSessionRequest {Email = body.NewEmail}, emailService);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendPasswordSession", Method.Post)]
    [Authentication(false)]
    public Response SendPasswordSession(RequestContext context, GameDatabaseContext database, ApiPasswordSessionRequest body, EmailService emailService)
    {
        GameUser? user = database.GetUserWithEmail(body.Email);
        if (user == null) return HttpStatusCode.Created; // trol

        string passwordSessionId = GeneratePasswordSessionId(database);
        GameSession passwordSession = database.CreateSession(user, SessionType.SetPassword, PlatformType.Api, 600, passwordSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your password code: " + passwordSession.Id + "\n" +
                           "If this wasn't you, feel free to ignore this email. Code expires in 10 minutes.";

        emailService.SendEmail(body.Email, "Sound Shapes Password Code", emailBody);

        return HttpStatusCode.Created;
    }
    
    [ApiEndpoint("account/setPassword", Method.Post)]
    public Response SetUserPassword(RequestContext context, GameDatabaseContext database, ApiSetPasswordRequest body, GameSession session)
    {
        GameUser user = session.User;

        if (!Sha512Regex().IsMatch(body.NewPasswordSha512))
            return new Response("Password is definitely not SHA512. Please hash the password.",
                ContentType.Plaintext, HttpStatusCode.BadRequest);

        database.SetUserPassword(user, body.NewPasswordSha512);
        database.RemoveSession(session);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/sendRemovalSession", Method.Post)]
    public Response SendUserRemovalSession(RequestContext context, GameDatabaseContext database, GameUser user, GameSession session, EmailService emailService)
    {
        if (user.Email == null)
            return new Response(
                "User does not have a linked email. Please contact a system administrator to remove your account.",
                ContentType.Plaintext, HttpStatusCode.Forbidden);
        
        string removalSessionId = GenerateAccountRemovalSessionId(database);
        GameSession removalSession = database.CreateSession(user, SessionType.RemoveAccount, PlatformType.Api, 600, removalSessionId); // 10 minutes

        string emailBody = $"Dear {user.Username},\n\n" +
                           "Here is your account removal code: " + removalSession.Id + "\n" +
                           "If this wasn't you, change your password immediately. Code expires in 10 minutes.";
        
        emailService.SendEmail(user.Email, "Sound Shapes Account Removal Code", emailBody);

        return HttpStatusCode.Created;
    }

    [ApiEndpoint("account/remove", Method.Post)]
    public Response RemoveAccount(RequestContext context, GameDatabaseContext database, GameUser user, IDataStore dataStore)
    {
        database.RemoveUser(user, dataStore);
        return new Response("o7", ContentType.Plaintext);
    }
}