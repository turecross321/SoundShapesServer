using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Sessions;
using SoundShapesServer.Responses.Users;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints;

public class AuthenticationEndpoints : EndpointGroup
{
    [Endpoint("/identity/login/token/psn", ContentType.Json, Method.Post)]
    [NullStatusCode(HttpStatusCode.Forbidden)]
    [Authentication(false)]
    public Response? Login(RequestContext context, RealmDatabaseContext database, Stream body)
    {
        Ticket ticket;
        try
        {
            ticket = Ticket.FromStream(body);
        }
        catch (Exception e)
        {
            context.Logger.LogWarning(BunkumContext.Authentication, "Could not read ticket: " + e);
            return null;
        }
        
        bool genuinePsn = ticket.IssuerId switch
        {
            0x100 => true, // ps3, ps4, psvita
            0x33333333 => false // rpcs3
        };

        string platform = PlatformHelper.GetPlatform(ticket.TitleId, genuinePsn).ToString();
        if (platform == PlatformType.unknown.ToString()) return null; 
        
        GameUser? user = database.GetUserWithDisplayName(ticket.Username);
        user ??= database.CreateUser(ticket.Username);
        
        Service? service = database.GetServiceWithDisplayName(ticket.Username);
        service ??= database.CreateService(ticket.Username);

        GameSession session = database.GenerateSessionForUser(user, platform, 14400); // 4 hours
        
        GameSessionResponse sessionResponse = new GameSessionResponse
        {
            ExpirationDate = session.Expires.ToUnixTimeMilliseconds(),
            Id = session.Id,
            User = new SessionUserResponse
            {
                DisplayName = user.DisplayName,
                Id = user.Id
            },
            Service = service
        };

        GameSessionWrapper responseWrapper = new ()
        {
            Session = sessionResponse
        };
        
        Console.WriteLine($"{sessionResponse.User.DisplayName} has logged in.");

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={session.Id};Version=1;Path=/");
        context.ResponseHeaders.Add("x-otg-identity-displayname", user.DisplayName);
        context.ResponseHeaders.Add("x-otg-identity-personid", user.Id);
        context.ResponseHeaders.Add("x-otg-identity-sessionid", session.Id);

        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }

    
    [Endpoint("/otg/~identity:*.hello", ContentType.Json)]
    public UserResponse Hello(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        return UserHelper.UserToUserResponse(user);
    }
    
}