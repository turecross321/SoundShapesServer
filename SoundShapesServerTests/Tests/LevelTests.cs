using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Requests.Api;
using SoundShapesServer.Responses.Api.Responses.Levels;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class LevelTests: ServerTest
{
    [Test]
    public async Task LevelFilteringWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user, "First Level", DateTimeOffset.UtcNow);
        context.CreateLevel(user, "Second Level", DateTimeOffset.UtcNow.AddDays(1));

        context.Database.Refresh();

        // Filtration
        IEnumerable<GameLevel> levels = context.Database.GetLevels(LevelOrderType.CreationDate, true, new LevelFilters(search: firstLevel.Name), null);
        Assert.That(levels.Count(), Is.EqualTo(1), "Check if filtration works");

        // Ordering
        levels = context.Database.GetLevels(LevelOrderType.CreationDate, true, new LevelFilters(), null);
        Assert.That(levels.First().CreationDate, Is.GreaterThan(levels.Last().CreationDate), "Check if ordering works");
        
        levels = context.Database.GetLevels(LevelOrderType.CreationDate, false, new LevelFilters(), null);
        Assert.That(levels.First().CreationDate, Is.LessThan(levels.Last().CreationDate), "Check if descending toggle works");
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

        // Search
        string payload = $"/otg/~index:level.page?query=metadata.displayName:{firstLevel.Name}";
        JObject response = JObject.Parse(await client.GetStringAsync(payload));
        
        Assert.That(response.GetValue("items")?.ToArray().Length, Is.EqualTo(1), "Check if search filter works");
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
        Assert.That(relationResponse is { Liked: false, Queued:false }, "Get relation of level");
        
        // Like Level
        string payload = $"/api/v1/levels/id/{level.Id}/like";
        HttpResponseMessage response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode, "Like level");
        
        // Check if Level was Liked
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: true, Queued:false }, "Get like status of level");
        
        // Unlike Level
        payload = $"/api/v1/levels/id/{level.Id}/unLike";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode, "Remove like from level");
        
        // Check if Like was removed
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:false }, "Check if like was removed from level");
        
        // Queueing
        payload = $"/api/v1/levels/id/{level.Id}/queue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode, "Queue level");
        
        // Check if Level got Queued
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:true }, "Check if level was queued");
        
        payload = $"/api/v1/levels/id/{level.Id}/unQueue";
        response = await client.PostAsync(payload, null);
        Assert.That(response.IsSuccessStatusCode, "Remove queue from level");
        
        // Check if Level was Un-queued
        relationResponse = 
            await client.GetFromJsonAsync<ApiLevelRelationResponse>(relationPayload);
        Assert.That(relationResponse is { Liked: false, Queued:false }, "Check if queue was removed from level");
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
        Assert.That(response.IsSuccessStatusCode, "Like level");
        
        payload = $"/otg/~identity:{user.Id}/~like:%2F~level%3A{level.Id}.get";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode, "Get like status of level");
        
        payload = $"/otg/~identity:{user.Id}/~like:%2F~level%3A{level.Id}.delete";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode, "Remove like from level");
        
        // Queueing
        
        // you can't queue levels through the game, hence this not being done through the HTTP client
        context.Database.QueueLevel(user, level);
        
        payload = $"/otg/~identity:{user.Id}/~queued:%2F~level%3A{level.Id}.get";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode, "Get queue status on level");
        
        payload = $"/otg/~identity:{user.Id}/~queued:%2F~level%3A{level.Id}.delete";
        response = await client.GetAsync(payload);
        Assert.That(response.IsSuccessStatusCode, "Remove queue from level");
    }

    [Test]
    public async Task ApiLevelManagingWorks()
    {
        using TestContext context = GetServer();
        
        GameUser firstUser = context.CreateUser();
        GameLevel firstLevel = context.CreateLevel(firstUser);
        
        GameUser secondUser = context.CreateUser();
        GameLevel secondLevel = context.CreateLevel(secondUser);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, firstUser);
        
        // Updating Metadata
        string payload = $"/api/v1/levels/id/{firstLevel.Id}/edit";
        ApiEditLevelRequest body = new()
        {
            Name = "Updated Level Name"
        };
        HttpResponseMessage response = await client.PostAsJsonAsync(payload, body);
        Assert.That(response.IsSuccessStatusCode, "Edit level");
        
        // Try changing metadata of other user's level
        payload = $"/api/v1/levels/id/{secondLevel.Id}/edit";
        body = new()
        {
            Name = "Updated Level Name"
        };
        response = await client.PostAsJsonAsync(payload, body);
        Assert.That(!response.IsSuccessStatusCode, "Edit other user's level");
        
        // Removing Level
        payload = $"/api/v1/levels/id/{firstLevel.Id}";
        response = await client.DeleteAsync(payload);
        Assert.That(response.IsSuccessStatusCode, "Remove level");
        
        // Try Removing other user's level
        payload = $"/api/v1/levels/id/{secondLevel.Id}";
        response = await client.DeleteAsync(payload);
        Assert.That(!response.IsSuccessStatusCode, "Remove other user's level");
    } 
}