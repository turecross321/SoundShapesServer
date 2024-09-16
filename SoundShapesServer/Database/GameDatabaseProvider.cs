using Bunkum.EntityFrameworkDatabase;
using SoundShapesServer.Types.Config;

namespace SoundShapesServer.Database;

public class GameDatabaseProvider(ServerConfig config)
    : EntityFrameworkDatabaseProvider<GameDatabaseContext>
{
    protected override EntityFrameworkInitializationStyle InitializationStyle { get; } = EntityFrameworkInitializationStyle.Migrate;

    public override GameDatabaseContext GetContext() => new(config);
}