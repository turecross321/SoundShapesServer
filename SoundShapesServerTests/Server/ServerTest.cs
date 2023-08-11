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
    protected TestContext GetServer(bool startServer = true)
    {
        DirectHttpListener listener = new();
        HttpClient client = listener.GetClient();

        InMemoryGameDatabaseProvider provider = new();
        provider.Initialize();

        Lazy<TestGameServer> testGameServer = new(() =>
        {
            TestGameServer gameServer = new(listener, provider);
            gameServer.Initialize();
            gameServer.Start();

            return gameServer;
        });

        if (startServer) _ = testGameServer.Value;
        
        return new TestContext(testGameServer, provider.GetContext(), client, listener);
    }
}