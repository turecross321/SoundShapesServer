using Bunkum.RealmDatabase;
using Realms;
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
    protected override ulong SchemaVersion => 31;

    protected override List<Type> SchemaTypes => new()
    {
        typeof(FollowRelation),
        typeof(LevelLikeRelation),
        typeof(LevelQueueRelation),
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
        typeof(GameEvent),
        typeof(CommunityTab)
    };

    protected override string Filename => "database.realm";

    public override void Warmup()
    {
        using GameDatabaseContext context = GetContext();
        _ = context.GetAdminUser();
    }

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        IQueryable<dynamic> oldLikeRelations = migration.OldRealm.DynamicApi.All("LevelLikeRelation");
        IQueryable<LevelLikeRelation> newLikeRelations = migration.NewRealm.All<LevelLikeRelation>();
        for (int i = 0; i < newLikeRelations.Count(); i++)
        {
            dynamic oldRelation = oldLikeRelations.ElementAt(i);
            LevelLikeRelation newRelation = newLikeRelations.ElementAt(i);

            if (oldVersion < 25)
            {
                newRelation.User = (GameUser)oldRelation.Liker;
            }
        }
        
        IQueryable<GameEvent> events = migration.NewRealm.All<GameEvent>();

        for (int i = 0; i < events.Count(); i++)
        {
            GameEvent gameEvent = events.ElementAt(i);
            
            // v28 added the Queue event type
            if (oldVersion < 28)
            {
                if (gameEvent.EventType >= (int)EventType.Queue)
                {
                    gameEvent.EventType += 1;
                }
            }
        }
        
        IQueryable<dynamic> oldLeaderboardEntries = migration.OldRealm.DynamicApi.All("LeaderboardEntry");
        IQueryable<LeaderboardEntry> newLeaderboardEntries = migration.NewRealm.All<LeaderboardEntry>();
        for (int i = 0; i < newLeaderboardEntries.Count(); i++)
        {
            dynamic oldEntry = oldLeaderboardEntries.ElementAt(i);
            LeaderboardEntry newEntry = newLeaderboardEntries.ElementAt(i);

            if (oldVersion < 29)
            {
                // Renamed Tokens to Notes
                newEntry.Notes = (int)oldEntry.Tokens;
            }
        }
    }
}