using System.Net;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests.Tests;

public class LeaderboardTests: ServerTest
{
    [Test]
    public async Task SubmitsLeaderboardEntry()
    {
        using TestContext context = GetServer();
        GameUser user = context.CreateUser();
        GameLevel level = context.CreateLevel(user);

        using HttpClient client = context.GetAuthenticatedClient(SessionType.Game, user);

        const string payload = $@"golded=1&playTime=0&tokenCount=8&date=0&deaths=0&score=0&completed=1";
        
        HttpResponseMessage message = 
            await client.PostAsync($"/otg/~identity:{user.Id}/~record:%2F~level%3A{level.Id}.post", 
                new StringContent(payload));
        
        Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        context.Database.Refresh();

        LeaderboardFilters filters = new(onLevel: level.Id, byUser:user);
        (IQueryable<LeaderboardEntry> entries, LeaderboardEntry[] _) =
            context.Database.GetLeaderboardEntries(LeaderboardOrderType.Score, false, filters, 0, 1);
        
        Assert.That(entries.ToList(), Has.Count.EqualTo(1));
    }
}