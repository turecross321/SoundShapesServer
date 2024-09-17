using Bunkum.Core.Storage;
using Bunkum.Protocols.Http.Direct;
using NotEnoughLogs;
using SoundShapesServer.Tests.Database;
using Testcontainers.PostgreSql;

namespace SoundShapesServer.Tests.Server;

[Parallelizable]
[CancelAfter(2000)]
public class ServerTest
{
    protected static readonly Logger Logger = new(new []
    {
        new NUnitSink(),
    });
    
    // ReSharper disable once MemberCanBeMadeStatic.Global
    protected SSTestContext GetServer(bool startServer = true, IDataStore? dataStore = null)
    {
        DirectHttpListener listener = new(Logger);
        HttpClient client = listener.GetClient();
        MockDateTimeProvider time = new();
        
        PostgreSqlContainer databaseContainer = new PostgreSqlBuilder().Build();
        databaseContainer.StartAsync().GetAwaiter().GetResult();
        TestDatabaseProvider provider = new(databaseContainer.GetConnectionString(), time);

        Lazy<TestSSServer> server = new(() =>
        {
            TestSSServer gameServer = new(listener, () => provider, dataStore);
            gameServer.Start();

            return gameServer;
        });

        if (startServer) _ = server.Value;
        else provider.Initialize();

        return new SSTestContext(server, provider.GetContext(), client, databaseContainer, listener, time);
    }
}