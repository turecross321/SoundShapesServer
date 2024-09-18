using Bunkum.Core.Storage;
using Bunkum.Protocols.Http;
using NotEnoughLogs;
using NotEnoughLogs.Behaviour;
using NotEnoughLogs.Sinks;
using SoundShapesServer.Common.Time;
using SoundShapesServer.Tests.Database;
using SoundShapesServer.Types.Config;

namespace SoundShapesServer.Tests.Server;

public class TestSSServer(BunkumHttpListener listener, Func<TestDatabaseProvider> provider, IDataStore? dataStore = null, ServerConfig? config = null)
    : SSServer<TestDatabaseProvider>(listener, provider, null, dataStore ?? new InMemoryDataStore(), config)
{
    public ServerConfig Config => this._config!;
    protected override IDateTimeProvider GetTimeProvider() => new MockDateTimeProvider();
    public override void Start()
    {
        this.Server.Start(0);
    }
    

    protected override (LoggerConfiguration logConfig, List<ILoggerSink>? sinks) GetLoggerConfiguration()
    {
        LoggerConfiguration logConfig = new()
        {
            Behaviour = new DirectLoggingBehaviour(),
            MaxLevel = LogLevel.Trace,
        };

        List<ILoggerSink> sinks = new(1)
        {
            new NUnitSink(),
        };
        
        return (logConfig, sinks);
    }
}