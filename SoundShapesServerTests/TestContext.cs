using Bunkum.Protocols.Http.Direct;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;
using SoundShapesServerTests.Server;

namespace SoundShapesServerTests;

public class TestContext : IDisposable
{
    public Lazy<TestGameServer> Server { get; }
    public GameDatabaseContext Database { get; }
    public HttpClient Http { get; }
    private DirectHttpListener Listener { get; }
    
    public TestContext(Lazy<TestGameServer> server, GameDatabaseContext database, HttpClient http, DirectHttpListener listener)
    {
        Server = server;
        Database = database;
        Http = http;
        Listener = listener;
    }
    
    private int _users;
    private int UserIncrement => _users++;

    public HttpClient GetAuthenticatedClient(SessionType type,
        GameUser? user = null,
        int tokenExpirySeconds = GameDatabaseContext.DefaultSessionExpirySeconds)
    {
        return GetAuthenticatedClient(type, out _, user, tokenExpirySeconds);
    }
    
    public HttpClient GetAuthenticatedClient(SessionType sessionType, out string sessionId,
        GameUser? user = null,
        int tokenExpirySeconds = GameDatabaseContext.DefaultSessionExpirySeconds)
    {
        user ??= CreateUser();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        PlatformType platformType = sessionType switch
        {
            SessionType.Game => PlatformType.PsVita,
            SessionType.Api => PlatformType.Api,
            _ => throw new ArgumentOutOfRangeException(nameof(sessionType), sessionType, null)
        };

        bool? genuineTicket = null; 
        if (sessionType == SessionType.Game)
            genuineTicket = true;
        
        GameSession session = Database.CreateSession(user, sessionType, platformType, genuineTicket, tokenExpirySeconds);
        sessionId = session.Id;
        
        HttpClient client = Listener.GetClient();

        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (sessionType is SessionType.Game or SessionType.Banned or SessionType.GameUnAuthorized)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-OTG-Identity-SessionId", session.Id);
        }
        else
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", session.Id);
        }

        return client;
    }

    public GameUser CreateUser(string? username = null)
    {
        username ??= UserIncrement.ToString();
        return Database.CreateUser(username);
    }
    
    public GameLevel CreateLevel(GameUser author, string title = "Level", DateTimeOffset? creationDate = null)
    {
        PublishLevelRequest request = new (title, 0, creationDate);
        return Database.CreateLevel(author, request, PlatformType.Unknown);
    }
    
    public void FillLeaderboard(GameLevel level, int userAmount, int scoresPerUser)
    {
        for (int i = userAmount; i > 0; i--)
        {
            GameUser scoreUser = Database.CreateUser("score" + i);
            for (int j = 0; j < scoresPerUser; j++)
            {
                SubmitLeaderboardEntry(i, level, scoreUser);
            }
        }
    }
    
    public LeaderboardEntry SubmitLeaderboardEntry(int score, GameLevel level, GameUser user)
    {
        LeaderboardSubmissionRequest request = new()
        {
            Score = score
        };

        LeaderboardEntry entry = Database.CreateLeaderboardEntry(request, user, level);

        return entry;
    }

    public void Dispose()
    {
        Database.Dispose();
        Http.Dispose();
        Listener.Dispose();

        if (!Server.IsValueCreated) 
            Server.Value.Stop();

        GC.SuppressFinalize(this);
    }
}