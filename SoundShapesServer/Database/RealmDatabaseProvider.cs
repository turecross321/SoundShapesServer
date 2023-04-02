using System.Diagnostics.CodeAnalysis;
using Bunkum.HttpServer.Database;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Requests;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

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
                typeof(LevelResources),
                typeof(ProfileMetadata),
                typeof(LevelMetadata),
                typeof(FollowRelation),
                typeof(LevelLikeRelation),
                typeof(GameUser),
                typeof(GameSession),
                typeof(Service),
                typeof(ExtraData),
                typeof(LevelParent),
                typeof(Metadata),
                typeof(GameLevel),
                typeof(NewsEntry)
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