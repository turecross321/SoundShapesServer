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
        
        HttpResponseMessage unAuthedRequest = await context.Http.GetAsync("/otg/ps3/SCEA/en/~eula.get");
        Assert.That(unAuthedRequest.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

        HttpClient authedClient = context.GetAuthenticatedClient(SessionType.Game, out string sessionId);
        
        GameSession? session = context.Database.GetSessionWithId(sessionId);
        Assert.That(session, Is.Not.Null);
        Assert.That(session?.User, Is.Not.Null);

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/otg/ps3/SCEA/en/~eula.get");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
    
    [Test]
    public async Task ApiAuthenticationWorks()
    {
        using TestContext context = GetServer();
        
        HttpResponseMessage unAuthedRequest = await context.Http.GetAsync("/api/v1/ip");
        Assert.That(unAuthedRequest.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));

        HttpClient authedClient = context.GetAuthenticatedClient(SessionType.Api, out string sessionId);
        
        GameSession? session = context.Database.GetSessionWithId(sessionId);
        Assert.That(session, Is.Not.Null);
        Assert.That(session?.User, Is.Not.Null);

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/api/v1/ip");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}