using Bunkum.RealmDatabase;
using Realms;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.PlayerActivity;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Sessions;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public class GameDatabaseProvider : RealmDatabaseProvider<GameDatabaseContext>
{
    protected override ulong SchemaVersion => 14;

    protected override List<Type> SchemaTypes => new()
    {
        typeof(FollowRelation),
        typeof(LevelLikeRelation),
        typeof(LevelPlayRelation),
        typeof(LevelUniquePlayRelation),
        typeof(GameUser),
        typeof(IpAuthorization),
        typeof(GameSession),
        typeof(GameLevel),
        typeof(NewsEntry),
        typeof(LeaderboardEntry),
        typeof(DailyLevel),
        typeof(GameAlbum),
        typeof(Report),
        typeof(Punishment),
        typeof(GameEvent)
    };

    protected override string Filename => "database.realm";

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        IQueryable<dynamic> oldLevels = migration.OldRealm.DynamicApi.All("GameLevel");
        IQueryable<GameLevel> newLevels = migration.NewRealm.All<GameLevel>();
        
        if (oldVersion < 13)
        {
            foreach (GameLevel level in newLevels)
            {
                level.LevelFilePath = ResourceHelper.GetLevelResourceKey(level.Id, FileType.Level);
                level.ThumbnailFilePath = ResourceHelper.GetLevelResourceKey(level.Id, FileType.Image);
                level.SoundFilePath = ResourceHelper.GetLevelResourceKey(level.Id, FileType.Sound);
            }

            foreach (GameAlbum album in migration.NewRealm.All<GameAlbum>())
            {
                album.ThumbnailFilePath = ResourceHelper.GetAlbumResourceKey(album.Id, AlbumResourceType.Thumbnail);
                album.SidePanelFilePath = ResourceHelper.GetAlbumResourceKey(album.Id, AlbumResourceType.SidePanel);
            }

            foreach (GameUser user in migration.NewRealm.All<GameUser>())
            {
                user.SaveFilePath = ResourceHelper.GetSaveResourceKey(user.Id);
            }

            foreach (NewsEntry newsEntry in migration.NewRealm.All<NewsEntry>())
            {
                newsEntry.ThumbnailFilePath = ResourceHelper.GetNewsResourceKey(newsEntry.Id);
            }
        }
    }
}