using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
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
    public async Task ApiLevelFetchingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user, "First Level", DateTimeOffset.UtcNow);
        context.CreateLevel(user, "Second Level", DateTimeOffset.UtcNow.AddDays(1));

        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);

        // Filtration
        string payload = $"/api/v1/levels?search={firstLevel.Name}";
        ApiLevelsWrapper? response = await client.GetFromJsonAsync<ApiLevelsWrapper>(payload);
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
    public async Task LevelFetchingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user, "First Level", DateTimeOffset.UtcNow);
        context.CreateLevel(user, "Second Level", DateTimeOffset.UtcNow.AddDays(1));

        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);

        // Filtration
        string payload = $"/otg/~index:level.page?query=metadata.displayName:{firstLevel.Name}";
        JObject response = JObject.Parse(await client.GetStringAsync(payload));
        
        Assert.That(response.GetValue("items")?.ToArray().Length, Is.EqualTo(1));
    }

    [Test]
    public async Task ApiLevelLikingAndQueuingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);
        
        // Check Relation
        string relationPayload = $"/api/v1/levels/id/{level.Id}/relationWith/id/{user.Id}";
        ApiLevelRelationResponse? relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:false });
        
        // Like Level
        string payload = $"/api/v1/levels/id/{level.Id}/like";
        HttpResponseMessage response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Check if Level got Liked
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: true, Queued:false });
        
        // Unlike Level
        payload = $"/api/v1/levels/id/{level.Id}/unLike";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Queueing
        payload = $"/api/v1/levels/id/{level.Id}/queue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Check if Level got Queued and UnLiked
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:true });
        
        payload = $"/api/v1/levels/id/{level.Id}/unQueue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode);
        
        // Check if Level got Un-queued
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:false });
    }
    
    [Test]
    public async Task LevelLikingAndQueuingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);
        
        // Liking
        string payload = $"/otg/~identity:{user.Id}/~like:%2F~level%3A{level.Id}.put";
        HttpResponseMessage response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
        
        payload = $"/otg/~identity:{user.Id}/~like:%2F~level%3A{level.Id}.get";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
        
        payload = $"/otg/~identity:{user.Id}/~like:%2F~level%3A{level.Id}.delete";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
        
        // Queueing
        
        // you can't queue levels through the game, hence this not being done through the HTTP client
        context.Database.QueueLevel(user, level);
        
        payload = $"/otg/~identity:{user.Id}/~queued:%2F~level%3A{level.Id}.get";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
        
        payload = $"/otg/~identity:{user.Id}/~queued:%2F~level%3A{level.Id}.delete";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
    }

    [Test]
    public async Task ApiLevelManagingWorks()
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
        payload = $"/api/v1/levels/id/{level.Id}";
        response = await client.DeleteAsync(payload);
        Assert.That(response.IsSuccessStatusCode);
        
        // Try Removing other user's level
        payload = $"/api/v1/levels/id/{otherLevel.Id}";
        response = await client.DeleteAsync(payload);
        Assert.That(!response.IsSuccessStatusCode);
    } 
}