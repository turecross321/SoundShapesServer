using System.Net;
using SoundShapesServer.Types.Sessions;
using SoundShapesServerTests.Server;
// ReSharper disable StringLiteralTypo

namespace SoundShapesServerTests.Tests;

public class AuthenticationTests : ServerTest
{
    [Test]
    public async Task GameAuthenticationWorks()
    {
        using TestContext context = GetServer();
        
        HttpResponseMessage unAuthedRequest = await context.Http.GetAsync("/otg/~index:activity.page");
        Assert.That(unAuthedRequest.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "Check if server is denying unauthenticated request");

        HttpClient authedClient = context.GetAuthenticatedClient(SessionType.Game, out string sessionId);
        
        GameSession? session = context.Database.GetSessionWithId(sessionId);
        Assert.That(session, Is.Not.Null, "Check if provided session exists");

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/otg/~index:activity.page");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Check if server is accepting authenticated request");
    }
    
    [Test]
    public async Task ApiAuthenticationWorks()
    {
        using TestContext context = GetServer();
        
        HttpResponseMessage unAuthedRequest = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(unAuthedRequest.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "Check if server is denying unauthenticated request");

        HttpClient authedClient = context.GetAuthenticatedClient(SessionType.Api, out string sessionId);
        
        GameSession? session = context.Database.GetSessionWithId(sessionId);
        Assert.That(session, Is.Not.Null);
        Assert.That(session?.User, Is.Not.Null);

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Check if server is accepting authenticated request");
    }
}