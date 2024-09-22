using System.Net.Http.Json;
using SoundShapesServer.Common.Constants;
using SoundShapesServer.Tests.Server;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Requests.Api;
using SoundShapesServer.Types.Responses.Api.ApiTypes;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Tests.Tests.Authentication;

public class TokenTests : ServerTest
{
    [Test]
    public void RevokeTokenWorks()
    {
        using SSTestContext context = this.GetServer();

        DbUser user = context.CreateUser();
        DbRefreshToken refreshToken = context.Database.CreateRefreshToken(user);

        List<DbToken> tokens = new();
        for (int i = 0; i < 5; i++)
        {
            tokens.Add(context.Database.CreateApiTokenWithRefreshToken(refreshToken));
        }

        HttpClient client = context.GetAuthenticatedClient(tokens.First());
        
        HttpResponseMessage response = client.PostAsync("/api/v1/revokeToken", null).Result;
        Assert.That(response.IsSuccessStatusCode);
        foreach (DbToken token in tokens)
        {
            Assert.That(context.Database.GetTokenWithId(token.Id), Is.EqualTo(null));
        }
        Assert.That(context.Database.GetRefreshTokenWithId(refreshToken.Id), Is.EqualTo(null));
    }

    [Test]
    public void TokenExpiryWorks()
    {
        using SSTestContext context = this.GetServer();

        HttpClient client = context.GetAuthenticatedClient(TokenType.ApiAccess);
        
        HttpResponseMessage response = client.GetAsync("/api/v1/users/me").Result;
        Assert.That(response.IsSuccessStatusCode);
        
        context.Time.Now = context.Time.Now.AddYears(1);

        response = client.GetAsync("/api/v1/users/me").Result;
        Assert.That(!response.IsSuccessStatusCode);
    }
    
    [Test]
    public void RefreshTokenExpiryWorks()
    {
        using SSTestContext context = this.GetServer();
        using HttpClient client = context.Http;

        DbUser user = context.CreateUser();
        DbRefreshToken refreshToken = context.Database.CreateRefreshToken(user);

        // get right before refresh token expires
        context.Time.Now = context.Time.Now.AddHours(ExpiryTimes.RefreshTokenHours).AddMinutes(-1);
        HttpResponseMessage response = client.PostAsJsonAsync("/api/v1/refreshToken", new ApiRefreshTokenRequest
        {
            RefreshTokenId = refreshToken.Id
        }).Result;
        Assert.That(response.IsSuccessStatusCode);
        
        // assure that the generated access token works
        ApiLoginResponse? deSerialized =
            response.Content.ReadFromJsonAsync<ApiResponse<ApiLoginResponse>>().Result!.Data;
        Assert.That(deSerialized != null);
        DbToken? accessToken = context.Database.GetTokenWithId(deSerialized!.AccessToken.Id);
        Assert.That(accessToken != null);
        
        // get right after the initial expiry date
        context.Time.Now = context.Time.Now.AddMinutes(2);
        
        response = client.PostAsJsonAsync("/api/v1/refreshToken", new ApiRefreshTokenRequest
        {
            RefreshTokenId = refreshToken.Id
        }).Result;

        // check that the refresh token still works since the expiry date should have been updated
        Assert.That(response.IsSuccessStatusCode);
        
        // go to after expiry date
        context.Time.Now = context.Time.Now.AddHours(ExpiryTimes.RefreshTokenHours).AddMinutes(1);
        response = client.PostAsJsonAsync("/api/v1/refreshToken", new ApiRefreshTokenRequest
        {
            RefreshTokenId = refreshToken.Id
        }).Result;

        Assert.That(!response.IsSuccessStatusCode);
        
        // check that the access token was generated with the refresh token has been revoked 
        using HttpClient authClient = context.GetAuthenticatedClient(accessToken!);
        response = client.GetAsync("/api/v1/users/me").Result;
        Assert.That(!response.IsSuccessStatusCode);
    }
}