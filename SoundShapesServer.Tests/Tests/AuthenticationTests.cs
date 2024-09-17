using System.Net.Http.Json;
using SoundShapesServer.Tests.Server;
using SoundShapesServer.Types;
using TestContext = NUnit.Framework.TestContext;

namespace SoundShapesServer.Tests.Tests;

public class AuthenticationTests: ServerTest
{
    [Test]
    public void AuthenticationEnforcementWorks()
    {
        using SSTestContext context = this.GetServer();

        using HttpClient client = context.Http;
        HttpResponseMessage result = client.GetAsync("/otg/ps3/SCEA/en/~eula.get").Result;
        Assert.That(!result.IsSuccessStatusCode);

        using HttpClient authClient = context.GetAuthenticatedClient(TokenType.GameEula);
        result = authClient.GetAsync("/otg/ps3/SCEA/en/~eula.get").Result;
        Assert.That(result.IsSuccessStatusCode);
    }
}