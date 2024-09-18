using Bunkum.Protocols.Http.Direct;
using SoundShapesServer.Database;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Database;
using Testcontainers.PostgreSql;

namespace SoundShapesServer.Tests.Server;

public class SSTestContext : IDisposable
{
    public Lazy<TestSSServer> Server { get; }
    public GameDatabaseContext Database { get; }
    public HttpClient Http { get; }
    public PostgreSqlContainer DatabaseContainer { get; }
    private DirectHttpListener Listener { get; }
    public MockDateTimeProvider Time { get; }
    
    private int _users = 100; // start at 100 since usernames require 3 characters
    private readonly PostgreSqlContainer _databaseContainer;

    public SSTestContext(Lazy<TestSSServer> server, GameDatabaseContext database, HttpClient http, PostgreSqlContainer databaseContainer, DirectHttpListener listener, MockDateTimeProvider time)
    {
        _databaseContainer = databaseContainer;
        Server = server;
        Database = database;
        Http = http;
        DatabaseContainer = databaseContainer;
        Listener = listener;
        Time = time;
    }

    private int UserIncrement => this._users++;
    
    public DbUser CreateUser(string? username = null, string email = "user@email.yep", UserRole role = UserRole.Default)
    {
        username ??= this.UserIncrement.ToString();
        DbUser user = this.Database.CreateRegisteredUser(username, email, role);
        return user;
    }

    public HttpClient GetAuthenticatedClient(TokenType type, PlatformType platform = PlatformType.PS3, DbUser? user = null)
    {
        user ??= this.CreateUser();

        DbToken token = Database.CreateToken(user, type, platform, null, null, null);

        return GetAuthenticatedClient(token);
    }
    
    public HttpClient GetAuthenticatedClient(DbToken token)
    {
        HttpClient client = this.Listener.GetClient();

        if (token.TokenType is TokenType.GameEula or TokenType.GameAccess)
        {
            client.DefaultRequestHeaders.Add("X-OTG-Identity-SessionId", token.Id.ToString());
        }
        else
        {
            client.DefaultRequestHeaders.Add("Authorization", token.Id.ToString());
        }

        return client;
    }
    public void Dispose()
    {
        this.Database.Dispose();
        this.Http.Dispose();
        this.Listener.Dispose();
        this.DatabaseContainer.DisposeAsync().AsTask();

        if (this.Server.IsValueCreated)
            this.Server.Value.Stop();

        GC.SuppressFinalize(this);
    }
}