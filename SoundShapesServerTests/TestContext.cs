using Bunkum.Protocols.Http.Direct;
using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
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

    public HttpClient GetAuthenticatedClient(TokenType type,
        GameUser? user = null,
        int tokenExpirySeconds = GameDatabaseContext.DefaultTokenExpirySeconds)
    {
        return GetAuthenticatedClient(type, out _, user, tokenExpirySeconds);
    }
    
    public HttpClient GetAuthenticatedClient(TokenType tokenType, out string tokenId,
        GameUser? user = null,
        int tokenExpirySeconds = GameDatabaseContext.DefaultTokenExpirySeconds)
    {
        user ??= CreateUser();

        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        PlatformType platformType = tokenType switch
        {
            TokenType.GameAccess => PlatformType.PsVita,
            TokenType.ApiAccess => PlatformType.Unknown,
            _ => throw new ArgumentOutOfRangeException(nameof(tokenType), tokenType, null)
        };

        bool? genuineTicket = null; 
        if (tokenType == TokenType.GameAccess)
            genuineTicket = true;
        
        GameToken token = Database.CreateToken(user, tokenType, TokenAuthenticationType.None, tokenExpirySeconds, platformType, genuineTicket);
        tokenId = token.Id;
        
        HttpClient client = Listener.GetClient();

        // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
        if (tokenType is TokenType.GameAccess or TokenType.GameUnAuthorized)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-OTG-Identity-SessionId", token.Id);
        }
        else
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", token.Id);
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
    
    public void FillLeaderboard(GameLevel level, int userAmount, int scoresPerUser, GameUser? firstPlaceUser = null)
    {
        for (int i = 0; i < userAmount; i++)
        {
            // if firstPlaceUser has been assigned, make the first user firstPlaceUser
            GameUser scoreUser = i == 0 && firstPlaceUser != null ? firstPlaceUser : Database.CreateUser("score" + i);
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

        LeaderboardEntry entry = Database.CreateLeaderboardEntry(request, user, level, PlatformType.Unknown);

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