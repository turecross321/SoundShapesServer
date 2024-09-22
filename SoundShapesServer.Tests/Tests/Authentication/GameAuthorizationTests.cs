using System.Net.Http.Json;
using SoundShapesServer.Tests.Server;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Requests.Api;
using SoundShapesServer.Types.Responses.Api.DataTypes;

namespace SoundShapesServer.Tests.Tests.Authentication;

public class GameAuthorizationTests : ServerTest
{
    [Test]
    public void SettingAuthorizationSettingsWorks()
    {
        using SSTestContext context = this.GetServer();
        using HttpClient http = context.GetAuthenticatedClient(TokenType.ApiAccess);

        ApiAuthorizationSettingsResponse? response = 
            http.GetFromJsonAsync<ApiAuthorizationSettingsResponse>("/api/v1/gameAuth").Result;
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.EqualTo(null));
            // Check that they are their default settings
            Assert.That(response!.RpcnAuthorization, Is.EqualTo(false));
            Assert.That(response.PsnAuthorization, Is.EqualTo(false));
            Assert.That(response.IpAuthorization, Is.EqualTo(false));
        });

        response = http.PutAsJsonAsync("/api/v1/gameAuth", new ApiAuthorizationSettingsRequest
        {
            RpcnAuthorization = true,
            PsnAuthorization = true,
            IpAuthorization = true
        }).Result.Content.ReadFromJsonAsync<ApiAuthorizationSettingsResponse>().Result;
        
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.EqualTo(null));
            // Check that they changed
            Assert.That(response!.RpcnAuthorization, Is.EqualTo(true));
            Assert.That(response.PsnAuthorization, Is.EqualTo(true));
            Assert.That(response.IpAuthorization, Is.EqualTo(true));
        });
    }
}