using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class LeaderboardTests: ServerTest
{
    [Test]
    public async Task ApiLeaderboardFetchingWorks()
    {
        const int usersOnFirstLevel = 10;
        const int usersOnSecondLevel = 20;
        const int scoresPerUser = 2;
        
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user);
        GameLevel secondLevel = context.CreateLevel(user);

        context.FillLeaderboard(firstLevel, usersOnFirstLevel, scoresPerUser);
        context.FillLeaderboard(secondLevel, usersOnSecondLevel, scoresPerUser);
        
        context.Database.Refresh();
        
        // Filtering
        
        // Only best entries on first level
        IEnumerable<LeaderboardEntry> entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(onlyBest: true, onLevel:firstLevel.Id));
        Assert.That(entries.Count(), Is.EqualTo(usersOnFirstLevel));
        
        // All entries on second level
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(onlyBest: false, onLevel:secondLevel.Id));
        Assert.That(entries.Count(), Is.EqualTo(usersOnSecondLevel * scoresPerUser));
        
        // All entries
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(onlyBest: false));
        Assert.That(entries.Count(), Is.EqualTo((usersOnFirstLevel + usersOnSecondLevel) * scoresPerUser));
        
        // Ordering
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(onlyBest: false));
        Assert.That(entries.First().Score, Is.LessThan(entries.Last().Score));
        
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            true, new LeaderboardFilters(onlyBest: false));
        Assert.That(entries.First().Score, Is.GreaterThan(entries.Last().Score));
    }
    
    [Test]
    public async Task LeaderboardFetchingWorks()
    {
        // WARNING: Can't be more than 9
        const int firstUserAmount = 5;
        const int secondUserAmount = 7;
        const int scoresPerUser = 2;
        
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        
        GameLevel firstLevel = context.CreateLevel(user);
        GameLevel secondLevel = context.CreateLevel(user);

        context.FillLeaderboard(firstLevel, firstUserAmount, scoresPerUser);
        context.FillLeaderboard(secondLevel, secondUserAmount, scoresPerUser);
        
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);
        
        // Fetching
        string payload = $"/otg/~level:{firstLevel.Id}/~leaderboard.page";
        JObject response = JObject.Parse(await client.GetStringAsync(payload));
        
        Assert.That(response.GetValue("items")?.ToArray().Length, Is.EqualTo(firstUserAmount));
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
        IQueryable<LeaderboardEntry> entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters);
        
        Assert.That(entries.Count(), Is.EqualTo(1));
    }
    
    private void LeaderboardSegmentTest(int expectedIndex, int amount, int score)
    {
        using TestContext context = GetServer();
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);

        context.FillLeaderboard(level, amount, 2);
        LeaderboardEntry entry = context.SubmitLeaderboardEntry(score, level, user);

        LeaderboardFilters filters = new(onLevel:level.Id);
        List<LeaderboardEntry> entries = 
            context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters).ToList();
        
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