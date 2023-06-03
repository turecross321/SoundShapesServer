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

    public BunkumHttpServer BunkumServerInstance => ServerInstance;
    public new GameDatabaseProvider DatabaseProvider => base.DatabaseProvider;

    protected override void SetUpConfiguration()
    {
        ServerInstance.UseConfig(new GameServerConfig());
    }
}