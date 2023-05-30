using Bunkum.CustomHttpListener.Listeners.Direct;
using Bunkum.HttpServer;
using NotEnoughLogs;
using NotEnoughLogs.Loggers;

namespace SoundShapesServerTests.Server;

[Timeout(2000)]
public class ServerTest
{
    protected static readonly LoggerContainer<BunkumContext> Logger = new();

    static ServerTest()
    {
        Logger.RegisterLogger(new ConsoleLogger());
    }
    
    // ReSharper disable once MemberCanBeMadeStatic.Global
    protected TestContext GetServer()
    {
        DirectHttpListener listener = new();
        HttpClient client = listener.GetClient();

        InMemoryGameDatabaseProvider provider = new();
        provider.Initialize();

        Lazy<TestServer> gameServer = new(() =>
        {
            TestServer server = new(listener, provider);
            server.Initialize();
            server.Start();

            return server;
        });

        return new TestContext(gameServer, provider.GetContext(), client, listener);
    }
}