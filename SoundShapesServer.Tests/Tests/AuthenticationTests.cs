using System.Net.Http.Json;
using SoundShapesServer.Tests.Server;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;
using TestContext = NUnit.Framework.TestContext;

namespace SoundShapesServer.Tests.Tests;

public class AuthenticationTests : ServerTest
{
    [Test]
    public void AuthenticationEnforcementWorks()
    {
        using SSTestContext context = this.GetServer();

        string apiEndpoint = "/api/v1/me";
        string gameEulaEndpoint = "/otg/ps3/SCEA/en/~eula.get";
        //string gameEndpoint = ""; // Todo: add any authenticated endpoint to this when there is one implemented


        foreach (TokenType tokenType in Enum.GetValuesAsUnderlyingType(typeof(TokenType)))
        {
            HttpClient client = context.GetAuthenticatedClient(tokenType);

            HttpResponseMessage response = client.GetAsync(apiEndpoint).Result;
            Assert.That(response.IsSuccessStatusCode, Is.EqualTo(tokenType == TokenType.ApiAccess));

            response = client.GetAsync(gameEulaEndpoint).Result;
            Assert.That(response.IsSuccessStatusCode,
                Is.EqualTo(tokenType is TokenType.GameEula or TokenType.GameAccess));

            //response = client.GetAsync(gameEndpoint).Result;
            //Assert.That(response.IsSuccessStatusCode, Is.EqualTo(tokenType is TokenType.GameAccess));
        }
    }

    // todo: fix the email service so that this test works
    /*
    [Test]
    public void ApiSetEmailWorks()
    {
        string name = "gamer2";
        string newEmail = "bigM@yep.yup";

        using SSTestContext context = this.GetServer();

        DbUser user = context.CreateUser(name);
        HttpClient http = context.GetAuthenticatedClient(TokenType.ApiAccess, user:user);

        HttpResponseMessage response = http.PutAsJsonAsync("/api/v1/setEmail", new ApiSetEmailRequest
        {
            NewEmail = newEmail
        }).Result;

        Assert.That(response.IsSuccessStatusCode);

        DbCode? code = context.Database.GetCode(user, CodeType.VerifyEmail);
        Assert.That(code != null);

        response = http.PutAsJsonAsync("verifyEmail", new ApiCodeRequest
        {
            Code = code!.Code
        }).Result;

        Assert.That(response.IsSuccessStatusCode);
        Assert.That(context.Database.GetUserWithName(name)!.EmailAddress == newEmail);
    }*/
}