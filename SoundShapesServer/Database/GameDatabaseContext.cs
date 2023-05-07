using Bunkum.RealmDatabase;

namespace SoundShapesServer.Database;

public partial class GameDatabaseContext : RealmDatabaseContext
{
    private static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }
}