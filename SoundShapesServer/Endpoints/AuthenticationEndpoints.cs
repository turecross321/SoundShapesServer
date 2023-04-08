using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using NPTicket;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses;
using SoundShapesServer.Responses.Sessions;
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

        string platform = PlatformHelper.GetPlatform(ticket.TitleId).ToString();
        if (platform == Platform.unknown.ToString()) return null; 
        
        GameUser? user = database.GetUserWithDisplayName(ticket.Username);
        user ??= database.CreateUser(ticket.Username);
        
        Service? service = database.GetServiceWithDisplayName(ticket.Username);
        service ??= database.CreateService(ticket.Username);

        GameSession session = database.GenerateSessionForUser(user, platform, 14400); // 4 hours
        
        GameSessionResponse sessionResponse = new GameSessionResponse
        {
            expires = session.expires.ToUnixTimeMilliseconds(),
            id = session.id,
            person = new SessionUserResponse
            {
                display_name = user.display_name,
                id = user.id
            },
            service = service
        };

        GameSessionWrapper responseWrapper = new ()
        {
            session = sessionResponse
        };
        
        Console.WriteLine($"{sessionResponse.person.display_name} has logged in.");

        context.ResponseHeaders.Add("set-cookie", $"OTG-Identity-SessionId={session.id};Version=1;Path=/");
        context.ResponseHeaders.Add("x-otg-identity-displayname", user.display_name);
        context.ResponseHeaders.Add("x-otg-identity-personid", user.id);
        context.ResponseHeaders.Add("x-otg-identity-sessionid", session.id);

        return new Response(responseWrapper, ContentType.Json, HttpStatusCode.Created);
    }

    
    [Endpoint("/otg/~identity:*.hello", ContentType.Json)]
    public UserResponse Hello(RequestContext context, RealmDatabaseContext database, GameUser user)
    {
        return UserHelper.ConvertGameUserToUserResponse(user);
    }
    
}