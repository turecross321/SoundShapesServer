using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using SoundShapesServer;
using SoundShapesServer.Configuration;
using SoundShapesServer.Database;

namespace SoundShapesServerTests.Server;

public class TestGameServer : GameServer
{
    public TestGameServer(BunkumHttpListener listener, GameDatabaseProvider provider) : base(listener, provider, null, new InMemoryDataStore())
    {
        Config = new GameServerConfig();
    }

    public BunkumHttpServer BunkumServerInstance => ServerInstance;
    public new GameDatabaseProvider DatabaseProvider => base.DatabaseProvider;
    
    public override void Start()
    {
        ServerInstance.Start(0);
    }
}