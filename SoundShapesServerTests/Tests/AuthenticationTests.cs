using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Requests.Api.Account;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses;
using SoundShapesServer.Types.Authentication;
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

        HttpClient authedClient = context.GetAuthenticatedClient(TokenType.GameAccess, out string tokenId);
        
        AuthToken? token = context.Database.GetTokenWithId(tokenId);
        Assert.That(token, Is.Not.Null, "Check if provided token exists");

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/otg/~index:activity.page");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Check if server is accepting authenticated request");
    }
    
    [Test]
    public async Task ApiAuthenticationWorks()
    {
        using TestContext context = GetServer();
        
        HttpResponseMessage unAuthedRequest = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(unAuthedRequest.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "Check if server is denying unauthenticated request");

        HttpClient authedClient = context.GetAuthenticatedClient(TokenType.ApiAccess, out string tokenId);
        
        AuthToken? token = context.Database.GetTokenWithId(tokenId);
        Assert.That(token, Is.Not.Null);
        Assert.That(token?.User, Is.Not.Null);

        HttpResponseMessage authedRequest = await authedClient.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(authedRequest.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Check if server is accepting authenticated request");
    }

    [Test]
    public async Task ApiTokenRefreshWorks()
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
        ApiTokenResponse accessToken = responseContent.Data!.AccessToken;
        ApiTokenResponse refreshToken = responseContent.Data!.RefreshToken!;
        
        context.Database.Refresh();

        Assert.That(context.Database.GetTokenWithId(accessToken.Id)!.TokenType == TokenType.ApiAccess,
            "Check if provided access token is actually an API access token");
        Assert.That(context.Database.GetTokenWithId(refreshToken.Id)!.TokenType == TokenType.ApiRefresh,
            "Check if provided refresh token is actually an API refresh token");
        
        context.Http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken.Id);
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Checks if provided access token works");
        
        context.Database.RemoveToken(context.Database.GetTokenWithId(accessToken.Id)!);
        context.Database.Refresh();
        context.Http.DefaultRequestHeaders.Remove("Authorization");
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden), "Checks if access token no longer works after deleted");
        
        payload = new ApiRefreshTokenRequest
        {
            RefreshTokenId = refreshToken.Id
        };
        response = await context.Http.PostAsync("/api/v1/account/refreshToken", new StringContent(JsonConvert.SerializeObject(payload)));
        
        responseContentString = await response.Content.ReadAsStringAsync();
        responseContent =
            JsonConvert.DeserializeObject<ApiResponse<ApiLoginResponse>>(responseContentString)!;
        accessToken = responseContent.Data!.AccessToken;
        
        context.Http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", accessToken.Id);
        response = await context.Http.GetAsync("/api/v1/gameAuth/settings");
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Checks if new access token works");
    }
}