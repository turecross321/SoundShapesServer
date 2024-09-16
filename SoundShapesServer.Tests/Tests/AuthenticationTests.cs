using System.Net.Http.Json;
using SoundShapesServer.Tests.Server;
using TestContext = NUnit.Framework.TestContext;

namespace SoundShapesServer.Tests.Tests;

public class AuthenticationTests: ServerTest
{
    [Test]
    public void LoginWorks()
    {
        //using SSTestContext context = this.GetServer();
        //context.CreateUser("bob1");
        //Task<HttpResponseMessage> response = context.Http.PostAsJsonAsync<object>("/otg/identity/login/token/psn.post", null);

        //Assert.That(!response.Result.IsSuccessStatusCode);
    }
}