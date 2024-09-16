
using Bunkum.EntityFrameworkDatabase;
using SoundShapesServer.Common.Time;

namespace SoundShapesServer.Database;
public class GameDatabaseProvider(IDateTimeProvider? timeProvider = null, string connectionString = "")
    : EntityFrameworkDatabaseProvider<GameDatabaseContext>
{
    protected override EntityFrameworkInitializationStyle InitializationStyle => EntityFrameworkInitializationStyle.Migrate;

    public override GameDatabaseContext GetContext()
    {
        return new GameDatabaseContext(connectionString, timeProvider ?? new SystemDateTimeProvider());
    }
}