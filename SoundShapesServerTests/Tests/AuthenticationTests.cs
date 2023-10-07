using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
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

    [Test]
    public async Task ApiSessionRefreshWorks()
    {
        using TestContext context = GetServer();

        const string email = "example@mail.com";
        const string password = "password";
        string passwordSha512 = ResourceHelper.HashString(password);
        
        GameUser user = context.CreateUser();
        context.Database.SetUserEmail(user, email);
        context.Database.SetUserPassword(user, passwordSha512);
        context.Database.Refresh();

        object payload = new ApiLoginRequest
        {
            Email = email,
            PasswordSha512 = passwordSha512
        };
        
        HttpResponseMessage response = await context.Http.PostAsync("/api/v1/account/logIn", new StringContent(JsonConvert.SerializeObject(payload)));
        Assert.That(response.IsSuccessStatusCode, "Log in");

        string responseContentString = await response.Content.ReadAsStringAsync();
        ApiResponse<ApiLoginResponse> responseContent =
            JsonConvert.DeserializeObject<ApiResponse<ApiLoginResponse>>(responseContentString)!;
        ApiSessionResponse accessSession = responseContent.Data!.Session;
        ApiSessionResponse refreshSession = responseContent.Data!.RefreshSession!;
        
        context.Database.Refresh();

        Assert.That(context.Database.GetSessionWithId(accessSession.Id)!.SessionType == SessionType.Api,
            "Check if provided access session is actually an API session");
        Assert.That(context.Database.GetSessionWithId(refreshSession.Id)!.SessionType == SessionType.ApiRefresh,
            "Check if provided refresh session is actually a refresh session");
        
        context.Http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessSession.Id);
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Checks if provided access session works");
        
        context.Database.RemoveSession(context.Database.GetSessionWithId(accessSession.Id)!);
        context.Database.Refresh();
        context.Http.DefaultRequestHeaders.Remove("Authorization");
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "Checks if access session no longer works after deleted");
        
        payload = new ApiRefreshSessionRequest
        {
            RefreshSessionId = refreshSession.Id
        };
        response = await context.Http.PostAsync("/api/v1/account/refreshSession", new StringContent(JsonConvert.SerializeObject(payload)));
        
        responseContentString = await response.Content.ReadAsStringAsync();
        responseContent =
            JsonConvert.DeserializeObject<ApiResponse<ApiLoginResponse>>(responseContentString)!;
        accessSession = responseContent.Data!.Session;
        
        context.Http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessSession.Id);
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Checks if new access session works");
    }
}