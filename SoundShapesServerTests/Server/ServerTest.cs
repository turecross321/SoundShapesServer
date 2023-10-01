using Bunkum.Protocols.Http.Direct;
using NotEnoughLogs;

namespace SoundShapesServerTests.Server;

[Parallelizable]
[Timeout(2000)]
public class ServerTest
{
    protected static readonly Logger Logger = new();
    
    // ReSharper disable once MemberCanBeMadeStatic.Global
    protected TestContext GetServer(bool startServer = true)
    {
        DirectHttpListener listener = new(Logger);
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