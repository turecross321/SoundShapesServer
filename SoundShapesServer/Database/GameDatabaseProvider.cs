using Bunkum.RealmDatabase;
using Realms;
using SoundShapesServer.Authentication;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.RecentActivity;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public class GameDatabaseProvider : RealmDatabaseProvider<GameDatabaseContext>
{
    protected override ulong SchemaVersion => 5;

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
        // Not used
    }
}