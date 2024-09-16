using Bunkum.EntityFrameworkDatabase;
using SoundShapesServer.Database;
using SoundShapesServer.Tests.Server;

namespace SoundShapesServer.Tests.Database;

public class TestDatabaseProvider(string connectionString, MockDateTimeProvider time) : GameDatabaseProvider
{
    protected override EntityFrameworkInitializationStyle InitializationStyle => EntityFrameworkInitializationStyle.Migrate;
    
    public override GameDatabaseContext GetContext()
    {
        return new GameDatabaseContext(connectionString, time);
    }
}