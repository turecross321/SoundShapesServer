using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Sessions;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using ContentType = Bunkum.CustomHttpListener.Parsing.ContentType;

namespace SoundShapesServer.Endpoints.Game;

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

        PlatformType platform = PlatformHelper.GetPlatform(ticket.TitleId, genuinePsn);
        if (platform == PlatformType.Unknown) return HttpStatusCode.Forbidden; 
        
        GameUser? user = database.GetUserWithUsername(ticket.Username);
        if (user == null) return HttpStatusCode.Forbidden;
        
        Service? service = database.GetServiceWithDisplayName(ticket.Username);
        service ??= database.CreateService(ticket.Username);

        Session session = database.GenerateSessionForUser(user, platform, 14400); // 4 hours
        
        GameSessionResponse sessionResponse = new GameSessionResponse
        {
            ExpirationDate = session.ExpiresAt.ToUnixTimeMilliseconds(),
            Id = session.Id,
            User = new SessionUserResponse
            {
                DisplayName = user.Username,
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
        context.ResponseHeaders.Add("x-otg-identity-displayname", user.Username);
        context.ResponseHeaders.Add("x-otg-identity-personid", user.Id);
        context.ResponseHeaders.Add("x-otg-identity-sessionid", session.Id);

        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }

    
    [GameEndpoint("~identity:*.hello", ContentType.Json)]
    public UserResponse Hello(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        return UserHelper.UserToUserResponse(user);
    }
    
}