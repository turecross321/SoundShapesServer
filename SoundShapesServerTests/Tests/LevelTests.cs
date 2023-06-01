using System.Net.Http.Json;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class LevelTests: ServerTest
{
    [Test]
    public async Task LevelFetchingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user, "First Level");
        GameLevel secondLevel = context.CreateLevel(user, "Second Level");

        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);

        // Filtration
        string payload = $"/api/v1/levels?search={firstLevel.Name}";
        ApiLevelsWrapper? response = await client.GetFromJsonAsync<ApiLevelsWrapper>(payload);
        Assert.That(response?.Count, Is.EqualTo(1));
        
        payload = $"/api/v1/levels?search={secondLevel.Name}";
        response = await client.GetFromJsonAsync<ApiLevelsWrapper>(payload);
        Assert.That(response?.Count, Is.EqualTo(1));
        
        // Ordering
        payload = $"/api/v1/levels?orderBy=creationDate&descending=true";
        response = await client.GetFromJsonAsync<ApiLevelsWrapper>(payload);
        
        Assert.That(response != null && response.Levels[0].CreationDate > response.Levels[1].CreationDate);
        
        payload = $"/api/v1/levels?orderBy=creationDate&descending=false";
        response = await client.GetFromJsonAsync<ApiLevelsWrapper>(payload);
        
        Assert.That(response != null && response.Levels[0].CreationDate < response.Levels[1].CreationDate);
    }

    [Test]
    public async Task LevelLikingAndQueuingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);
        
        // Liking
        string payload = $"/api/v1/levels/id/{level.Id}/like";
        HttpResponseMessage response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        payload = $"/api/v1/levels/id/{level.Id}/unLike";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Queueing
        payload = $"/api/v1/levels/id/{level.Id}/queue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        payload = $"/api/v1/levels/id/{level.Id}/unQueue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
    }

    [Test]
    public async Task LevelManagingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);
        
        GameUser otherUser = context.CreateUser();
        GameLevel otherLevel = context.CreateLevel(otherUser);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);
        
        // Updating Metadata
        string payload = $"/api/v1/levels/id/{level.Id}/edit";
        ApiPublishLevelRequest body = new()
        {
            Name = "Updated Level Name"
        };
        HttpResponseMessage response = await client.PostAsJsonAsync(payload, body);
        Assert.That(response.IsSuccessStatusCode);
        
        // Try changing metadata of other user's level
        payload = $"/api/v1/levels/id/{otherLevel.Id}/edit";
        body = new()
        {
            Name = "Updated Level Name"
        };
        response = await client.PostAsJsonAsync(payload, body);
        Assert.That(!response.IsSuccessStatusCode);
        
        // Removing Level
        payload = $"/api/v1/levels/id/{level.Id}/remove";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Try Removing other user's level
        payload = $"/api/v1/levels/id/{otherLevel.Id}/remove";
        response = await client.PostAsync(payload, null);
        Assert.That(!response.IsSuccessStatusCode);
    } 
}