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
    public async Task LevelFilteringWorks()
    {
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user, "First Level");
        GameLevel secondLevel = context.CreateLevel(user, "Second Level");

        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);
        
        string firstPayload = $"/api/v1/levels?search={firstLevel.Name}";
        ApiLevelsWrapper? firstResponse = await client.GetFromJsonAsync<ApiLevelsWrapper>(firstPayload);
        Assert.That(firstResponse?.Count, Is.EqualTo(1));
        
        string secondPayload = $"/api/v1/levels?search={secondLevel.Name}";
        ApiLevelsWrapper? secondResponse = await client.GetFromJsonAsync<ApiLevelsWrapper>(secondPayload);
        Assert.That(secondResponse?.Count, Is.EqualTo(1));
    }
}