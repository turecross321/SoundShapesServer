using Bunkum.Core.Storage;
using Bunkum.Protocols.Http.Direct;
using SoundShapesServer.Common.Types;
using SoundShapesServer.Common.Types.Database;
using SoundShapesServer.Database;
using SoundShapesServer.Tests.Database;

namespace SoundShapesServer.Tests.Server;

public class SSTestContext(Lazy<TestSSServer> server, GameDatabaseContext database, HttpClient http, DirectHttpListener listener, MockDateTimeProvider time) : IDisposable
{
    public Lazy<TestSSServer> Server { get; } = server;
    public GameDatabaseContext Database { get; } = database;
    public HttpClient Http { get; } = http;
    private DirectHttpListener Listener { get; } = listener;
    public MockDateTimeProvider Time { get; } = time;
    
    private int _users = 100; // start at 100 since usernames require 3 characters
    private int UserIncrement => this._users++;
    
    public DbUser CreateUser(string? username = null)
    {
        username ??= this.UserIncrement.ToString();
        return this.Database.CreateUser(username);
    }
    
    public HttpClient GetAuthenticatedClient(TokenType type, PlatformType platform, out DbToken token,
        DbUser? user = null)
    {
        user ??= this.CreateUser();

        token = Database.CreateToken(user, type, platform);
        
        HttpClient client = this.Listener.GetClient();

        if (type is TokenType.GameEula or TokenType.GameAccess)
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

        if (this.Server.IsValueCreated)
            this.Server.Value.Stop();

        GC.SuppressFinalize(this);
    }
}