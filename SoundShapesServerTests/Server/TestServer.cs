using Bunkum.CustomHttpListener;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Storage;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;

namespace SoundShapesServerTests.Server;

public class TestServer: SoundShapesServer.Server
{
    public TestServer(BunkumHttpListener listener, GameDatabaseProvider provider) : base(listener, provider, null, new InMemoryDataStore())
    {}

    public BunkumHttpServer BunkumServer => _server;
    public GameDatabaseProvider DatabaseProvider => _databaseProvider;

    protected override void SetUpConfiguration()
    {
        _server.UseConfig(new GameServerConfig());
    }
}