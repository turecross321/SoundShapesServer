using System.Net;
using Newtonsoft.Json;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class PermissionTests : ServerTest
{
    [Test]
    public async Task PermissionsAttributeWorks()
    {
        TestContext context = GetServer();
        GameUser user = context.CreateUser();
        HttpClient client = context.GetAuthenticatedClient(TokenType.ApiAccess, user);
        
        // Administrator test
        context.Database.SetUserPermissions(user, PermissionsType.Default);
        context.Database.Refresh();
        object payload = new ApiCreateLevelRequest
        {
            Name = "Level"
        };
        HttpResponseMessage response = await client.PostAsync("/api/v1/levels/create", new StringContent(JsonConvert.SerializeObject(payload)));
        Assert.That(response.StatusCode == HttpStatusCode.Unauthorized, "Check if default user can access admin endpoint");
        
        context.Database.SetUserPermissions(user, PermissionsType.Administrator);
        context.Database.Refresh();
        
        response = await client.PostAsync("/api/v1/levels/create", new StringContent(JsonConvert.SerializeObject(payload)));
        Assert.That(response.StatusCode == HttpStatusCode.OK, "Check if admin can access admin endpoint");
        GameLevel publishedLevel = user.Levels.First();
        
        // Banned test
        context.Database.SetUserPermissions(user, PermissionsType.Banned);
        context.Database.Refresh();

        payload = new ApiEditLevelRequest
        {
            Name = "Level!"
        };
        response = await client.PostAsync($"/api/v1/levels/id/{publishedLevel.Id}/edit",
            new StringContent(JsonConvert.SerializeObject(payload)));
        Assert.That(response.StatusCode == HttpStatusCode.Unauthorized, "Check if banned user can access default endpoint");

        response = await client.GetAsync("api/v1/levels");
        Assert.That(response.StatusCode == HttpStatusCode.OK, "Check if banned user can still access no authentication endpoint");
        
        // Default test
        context.Database.SetUserPermissions(user, PermissionsType.Default);
        context.Database.Refresh();
        
        response = await client.PostAsync($"/api/v1/levels/id/{publishedLevel.Id}/edit",
            new StringContent(JsonConvert.SerializeObject(payload)));
        Assert.That(response.StatusCode == HttpStatusCode.OK, "Check if default user can access default endpoint");
    }
}