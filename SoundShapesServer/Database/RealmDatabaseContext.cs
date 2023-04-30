using Realms;
using Bunkum.HttpServer.Database;

namespace SoundShapesServer.Database;

public partial class RealmDatabaseContext : IDatabaseContext
{
    private readonly Realm _realm;

    internal RealmDatabaseContext(Realm realm)
    {
        this._realm = realm;
    }
    
    private static string GenerateGuid()
    {
        Guid guid = Guid.NewGuid();
        return guid.ToString();
    }

    public void Dispose()
    {
        //NOTE: we dont dispose the realm here, because the same thread may use it again, so we just `Refresh()` it
        this._realm.Refresh();
    }
}