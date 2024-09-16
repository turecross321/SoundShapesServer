using Bunkum.EntityFrameworkDatabase;
using SoundShapesServer.Database;
using SoundShapesServer.Tests.Server;

namespace SoundShapesServer.Tests.Database;

public class TestDatabaseProvider(MockDateTimeProvider time) : GameDatabaseProvider
{
    protected override EntityFrameworkInitializationStyle InitializationStyle => EntityFrameworkInitializationStyle.Migrate;
    
    public override TestDatabaseContext GetContext()
    {
        return new TestDatabaseContext(time);
    }
}