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
        _configuration = new RealmConfiguration(Path.Join(Environment.CurrentDirectory, "database.realm"))
        {
            SchemaVersion = 1,
            Schema = new []
            {
                typeof(FollowRelation),
                typeof(LevelLikeRelation),
                typeof(GameUser),
                typeof(IpAuthorization),
                typeof(GameSession),
                typeof(LevelParent),
                typeof(GameLevel),
                typeof(NewsEntry),
                typeof(LeaderboardEntry),
                typeof(DailyLevel),
                typeof(GameAlbum),
                typeof(Report),
                typeof(Punishment)
            },
            MigrationCallback = (_, _) =>
            {
            }
        };
    }

    private readonly ThreadLocal<Realm> _realmStorage = new(true);
    public RealmDatabaseContext GetContext() 
    {
        _realmStorage.Value ??= Realm.GetInstance(_configuration);
        
        return new RealmDatabaseContext(_realmStorage.Value);
    }
    
    public void Dispose() 
    {
        foreach (Realm realmStorageValue in _realmStorage.Values) 
        {
            realmStorageValue.Dispose();
        }

        _realmStorage.Dispose();
    }
}