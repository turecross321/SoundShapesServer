using System.Net.Http.Json;
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
}