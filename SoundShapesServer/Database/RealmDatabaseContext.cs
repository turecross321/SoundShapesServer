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
    
    private static readonly object IdLock = new();
    private void AddSequentialObject<T>(T obj, IList<T>? list = null, Action? writtenCallback = null) where T : IRealmObject
    {
        lock (IdLock)
        {
            this._realm.Write(() =>
            {
                int newId = this._realm.All<T>().Count() + 1;

                this._realm.Add(obj);
                if(list == null) writtenCallback?.Invoke();
            });
        }
        
        if (list != null)
        {
            this._realm.Write(() =>
            {
                list.Add(obj);
                writtenCallback?.Invoke();
            });
        }
    }

    private void AddSequentialObject<T>(T obj, Action? writtenCallback = null) where T : IRealmObject
        => this.AddSequentialObject(obj, null, writtenCallback);
    
    private static long GetTimestampMilliseconds() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}