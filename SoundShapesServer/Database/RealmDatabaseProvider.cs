using Bunkum.HttpServer.Database;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Relations;

namespace SoundShapesServer.Database;

public class RealmDatabaseProvider : IDatabaseProvider<RealmDatabaseContext>
{
    private RealmConfiguration _configuration = null!;
    
    public void Initialize()
    {
        this._configuration = new RealmConfiguration(Path.Join(Environment.CurrentDirectory, "database.realm"))
        {
            SchemaVersion = 1,
            Schema = new []
            {
                typeof(FollowRelation),
                typeof(LevelLikeRelation),
                typeof(GameUser),
                typeof(GameSession),
                typeof(Service),
                typeof(LevelParent),
                typeof(GameLevel),
                typeof(NewsEntry),
                typeof(LeaderboardEntry),
                typeof(DailyLevel),
                typeof(LinerNote),
                typeof(GameAlbum)
            },
            MigrationCallback = (migration, oldVersion) =>
            {
            }
        };
    }

    private readonly ThreadLocal<Realm> _realmStorage = new(true);
    public RealmDatabaseContext GetContext() 
    {
        this._realmStorage.Value ??= Realm.GetInstance(this._configuration);
        
        return new RealmDatabaseContext(this._realmStorage.Value);
    }
    
    public void Dispose() 
    {
        foreach (Realm realmStorageValue in this._realmStorage.Values) 
        {
            realmStorageValue?.Dispose();
        }

        this._realmStorage.Dispose();
    }
}