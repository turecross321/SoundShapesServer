using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game;
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
            false, new LeaderboardFilters(obsolete: true, onLevel:firstLevel), null);
        Assert.That(entries.Count(), Is.EqualTo(usersOnFirstLevel));
        
        // All entries on second level
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(obsolete: null, onLevel:secondLevel), null);
        Assert.That(entries.Count(), Is.EqualTo(usersOnSecondLevel * scoresPerUser));
        
        // Ordering
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            false, new LeaderboardFilters(obsolete: null, onLevel:firstLevel), null);
        Assert.That(entries.First().Score, Is.LessThan(entries.Last().Score));
        
        entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score,
            true, new LeaderboardFilters(obsolete: null, onLevel:secondLevel), null);
        Assert.That(entries.First().Score, Is.GreaterThan(entries.Last().Score));
    }
    
    [Test]
    public async Task LeaderboardFetchingWorks()
    {
        const int userAmount = 5;
        const int scoresPerUser = 4;
        
        using TestContext context = GetServer();
        
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);
        context.FillLeaderboard(level, userAmount, scoresPerUser, user);
        context.Database.Refresh();
        
        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);
        
        string payload = $"/otg/~level:{level.Id}/~leaderboard.page";
        ListResponse<LeaderboardEntryResponse> response = JsonConvert.DeserializeObject<ListResponse<LeaderboardEntryResponse>>(await client.GetStringAsync(payload))!;
        
        Assert.That(response.Count == userAmount, "Check if all scores were included");
        Assert.That(response.Items.First().Position == 1, "Check if position has been correctly set");
        Assert.That(response.Items.First().Entrant?.Id != response.Items[1].Entrant?.Id, "Check if obsolete scores are shown");

        payload = $"/otg/{level.Id}/~leaderboard.near";
        LeaderboardEntryResponse[] nearResponse = JsonConvert.DeserializeObject<LeaderboardEntryResponse[]>(await client.GetStringAsync(payload))!;
        Assert.That(nearResponse.First().Position == 1, "Check if near leaderboard positions are working properly");
        Assert.That(nearResponse.Length == 1, "Check if near leaderboard filters are working properly");
        Assert.That(IdHelper.DeFormatIdentityId(nearResponse.First().Entrant!.Id) == user.Id, "Check if near leaderboard is showing the correct user");
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

        LeaderboardFilters filters = new(onLevel: level, byUser:user);
        IQueryable<LeaderboardEntry> entries = context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters, null);
        
        Assert.That(entries.Count(), Is.EqualTo(1));
    }
}