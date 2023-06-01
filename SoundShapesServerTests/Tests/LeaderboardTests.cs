using System.Net;
using System.Net.Http.Json;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class LeaderboardTests: ServerTest
{
    [Test]
    public async Task LeaderboardFetchingWorks()
    {
        const int firstAmount = 10;
        const int secondAmount = 20;
        
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user);
        GameLevel secondLevel = context.CreateLevel(user);

        context.FillLeaderboard(firstLevel, firstAmount);
        context.FillLeaderboard(secondLevel, secondAmount);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Api, user);
        
        // Filtering
        string payload = $"/api/v1/scores?onLevel={firstLevel.Id}";
        ApiLeaderboardEntriesWrapper? response = await client.GetFromJsonAsync<ApiLeaderboardEntriesWrapper>(payload);
        Assert.That(response?.Count, Is.EqualTo(firstAmount));
        
        payload = $"/api/v1/scores?onLevel={secondLevel.Id}";
        response = await client.GetFromJsonAsync<ApiLeaderboardEntriesWrapper>(payload);
        Assert.That(response?.Count, Is.EqualTo(secondAmount));
        
        // Ordering
        payload = $"/api/v1/scores?orderBy=score&descending=true";
        response = await client.GetFromJsonAsync<ApiLeaderboardEntriesWrapper>(payload);
        Assert.That(response != null && response.Entries[0].Score >= response.Entries[1].Score);
        
        payload = $"/api/v1/scores?orderBy=score&descending=false";
        response = await client.GetFromJsonAsync<ApiLeaderboardEntriesWrapper>(payload);
        Assert.That(response != null && response.Entries[0].Score <= response.Entries[1].Score);
    }
    
    [Test]
    public async Task SubmitsLeaderboardEntry()
    {
        using TestContext context = GetServer();
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);

        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);

        const string payload = $@"golded=1&playTime=0&tokenCount=0&date=0&deaths=0&score=0&completed=1";
        
        HttpResponseMessage message = 
            await client.PostAsync($"/otg/~identity:{user.Id}/~record:%2F~level%3A{level.Id}.post", 
                new StringContent(payload));
        
        Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        context.Database.Refresh();

        LeaderboardFilters filters = new(onLevel: level.Id, byUser:user);
        (IQueryable<LeaderboardEntry> entries, LeaderboardEntry[] _) =
            context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters, 0, 0);
        
        Assert.That(entries.ToList(), Has.Count.EqualTo(1));
    }
    
    private void LeaderboardSegmentTest(int expectedIndex, int amount, int score)
    {
        using TestContext context = GetServer();
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);

        context.FillLeaderboard(level, amount);
        LeaderboardEntry entry = context.SubmitLeaderboardEntry(score, level, user);

        LeaderboardFilters filters = new(onLevel:level.Id);
        (IQueryable<LeaderboardEntry> entriesQueryable, LeaderboardEntry[] _) = 
            context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters, 0, 0);

        List<LeaderboardEntry> entries = entriesQueryable.ToList();
        
        Assert.Multiple(() =>
        {
            Assert.That(entries.IndexOf(entry), Is.EqualTo(expectedIndex));
            Assert.That(entries[expectedIndex], Is.EqualTo(entry));
        });
    }
    
    [Test]
    public void PlacesScoreInSegmentCorrectly()
    {
        const int amount = 20;
        const int expectedIndex = amount / 2;
        const int score = expectedIndex;
        
        LeaderboardSegmentTest(expectedIndex, amount, score);
    }
}