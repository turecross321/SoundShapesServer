using Bunkum.Core;
using Bunkum.RealmDatabase;
using MongoDB.Bson;
using Realms;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Authentication;
using SoundShapesServer.Types.Events;
using SoundShapesServer.Types.Leaderboard;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Levels.SSLevel;
using SoundShapesServer.Types.News;
using SoundShapesServer.Types.Punishments;
using SoundShapesServer.Types.Relations;
using SoundShapesServer.Types.Reports;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Database;

public class GameDatabaseProvider : RealmDatabaseProvider<GameDatabaseContext>
{
    protected override ulong SchemaVersion => 78;

    protected override List<Type> SchemaTypes => new()
    {
        typeof(UserFollowRelation),
        typeof(LevelLikeRelation),
        typeof(LevelQueueRelation),
        typeof(LevelPlayRelation),
        typeof(LevelUniquePlayRelation),
        typeof(GameUser),
        typeof(GameIp),
        typeof(GameToken),
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
    private readonly string DataStorePath = Path.Combine(BunkumFileSystem.DataDirectory, "dataStore");
    
    public override void Warmup()
    {
        using GameDatabaseContext context = GetContext();
        _ = context.GetAdminUser();
    }

    protected override void Migrate(Migration migration, ulong oldVersion)
    {
        GameUser adminUser = migration.NewRealm.All<GameUser>().First(u => u.Id == GameDatabaseContext.AdminId);
        
        IQueryable<dynamic> oldUsers = migration.OldRealm.DynamicApi.All("GameUser");
        IQueryable<GameUser> newUsers = migration.NewRealm.All<GameUser>();
        for (int i = 0; i < newUsers.Count(); i++)
        {
            dynamic oldUser = oldUsers.ElementAt(i);
            GameUser newUser = newUsers.ElementAt(i);

            if (oldVersion < 34)
            {
                newUser._PermissionsType = (int)oldUser.PermissionsType;
            }

            if (oldVersion < 42)
            {
                newUser.EventsCount = newUser.Events.Count();
            }

            if (oldVersion < 46)
            {
                newUser.Email = newUser.Email?.ToLower();
            }
        }
        
        foreach (string offlineLevelId in LevelHelper.OfflineLevelIds)
        {
            GameLevel level = migration.NewRealm.All<GameLevel>().First(l => l.Id == offlineLevelId);
            if (oldVersion < 56)
            {
                // Change the Visibility of offline levels to Unlisted instead of Private
                level.Visibility = LevelVisibility.Unlisted;
            }
        }
        
        IQueryable<dynamic> oldLevels = migration.OldRealm.DynamicApi.All("GameLevel");
        IQueryable<GameLevel> newLevels = migration.NewRealm.All<GameLevel>();
        if (oldVersion < 78)
        {
            for (int i = 0; i < newLevels.Count(); i++)
            {
                Console.WriteLine($"Getting hasUfo & hasFirefly data from levels... ({i}/{newLevels.Count()})");
                dynamic oldLevel = oldLevels.ElementAt(i);
                GameLevel newLevel = newLevels.ElementAt(i);

                if (!File.Exists(newLevel.LevelFilePath))
                    continue;
                byte[] levelFile = File.ReadAllBytes(newLevel.LevelFilePath);
                
                SSLevel? ssLevel = SSLevel.FromLevelFile(levelFile);
                if (ssLevel == null)
                    continue;
                newLevel.HasUfo = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_UFOCheckpoint");
                newLevel.HasFirefly = ssLevel.EntitiesB.Any(e => e.EntityType == "Platformer_EntityPacks_GameStuff_FireflyCheckpoint");
            }
        }
        
        migration.OldRealm.DynamicApi.All("GameAlbum");
        IQueryable<GameAlbum> newAlbums = migration.NewRealm.All<GameAlbum>();
        for (int i = 0; i < newAlbums.Count(); i++)
        {
            GameAlbum newAlbum = newAlbums.ElementAt(i);
            
            if (oldVersion < 44)
            {
                // just make it empty because i cant be bothered making a html to xml thing just for a migration
                newAlbum.LinerNotes = "<linerNotes></linerNotes>";
            }

            if (oldVersion < 75)
            {
                newAlbum.Author = migration.NewRealm.All<GameUser>().First(u => u.Id == GameDatabaseContext.AdminId);
            }
        }
        
        IQueryable<GameToken> newTokens = migration.NewRealm.All<GameToken>();
        for (int i = 0; i < newTokens.Count(); i++)
        {
            GameToken newToken = newTokens.ElementAt(i);

            if (oldVersion < 77)
            {
                migration.NewRealm.Remove(newToken);
            }
        }
        
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
        
        IQueryable<dynamic> oldQueueRelations = migration.OldRealm.DynamicApi.All("LevelQueueRelation");
        IQueryable<LevelQueueRelation> newQueueRelations = migration.NewRealm.All<LevelQueueRelation>();
        for (int i = 0; i < newQueueRelations.Count(); i++)
        {
            dynamic oldRelation = oldQueueRelations.ElementAt(i);
            LevelQueueRelation newRelation = newQueueRelations.ElementAt(i);
        }

        IQueryable<dynamic> oldFollowRelations;
        if (oldVersion < 72) 
            oldFollowRelations = migration.OldRealm.DynamicApi.All("FollowRelation");
        else 
            oldFollowRelations = migration.OldRealm.DynamicApi.All("UserFollowRelation");
        
        IQueryable<UserFollowRelation> newFollowRelations = migration.NewRealm.All<UserFollowRelation>();
        for (int i = 0; i < newFollowRelations.Count(); i++)
        {
            dynamic oldRelation = oldFollowRelations.ElementAt(i);
            UserFollowRelation newRelation = newFollowRelations.ElementAt(i);
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

            if (oldVersion < 47)
            {
                // Renamed Date to CreationDate
                newEntry.CreationDate = (DateTimeOffset)oldEntry.Date;
            }

            if (oldVersion < 52)
            {
                // Replaced LevelId with Level
                string levelId = (string)oldEntry.LevelId;

                GameLevel? level = migration.NewRealm.All<GameLevel>().FirstOrDefault(l => l.Id == levelId);
                if (level == null)
                {
                    level = new GameLevel
                    {
                        Author = adminUser,
                        Id = levelId,
                        Name = levelId,
                        Visibility = LevelVisibility.Private
                    };
                    migration.NewRealm.Add(level);
                }

                newEntry.Level = level;
            }

            if (oldVersion < 69)
            {
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (newEntry.User == null)
                    migration.NewRealm.Remove(newEntry);
            }

            if (oldVersion < 72)
            {
                // switched from sha512 string to objectid
                // this is a dumb workaround to keep the ids
                string id = (string)oldEntry.Id;
                id = new string(id.Where(c => c != '-').Take(24).ToArray());
                newEntry.Id = ObjectId.Parse(id);
            }
        }

        IQueryable<dynamic> oldPunishments = migration.OldRealm.DynamicApi.All("Punishment");
        IQueryable<Punishment> newPunishments = migration.NewRealm.All<Punishment>();
        for (int i = 0; i < newPunishments.Count(); i++)
        {
            dynamic oldPunishment = oldPunishments.ElementAt(i);
            Punishment newPunishment = newPunishments.ElementAt(i);

            if (oldVersion < 37)
            {
                newPunishment._PunishmentType = (int)oldPunishment.PunishmentType;
            }
            
            if (oldVersion < 40)
            {
                newPunishment.CreationDate = (DateTimeOffset)oldPunishment.IssuedAt;
                newPunishment.ExpiryDate = (DateTimeOffset)oldPunishment.ExpiresAt;
                newPunishment.RevokeDate = (DateTimeOffset)oldPunishment.RevokedAt;
            }

            if (oldVersion < 50)
            {
                // Added ModificationDate
                newPunishment.ModificationDate = newPunishment.CreationDate;
            }
        }
        
        IQueryable<dynamic> oldReports = migration.OldRealm.DynamicApi.All("Report");
        IQueryable<Report> newReports = migration.NewRealm.All<Report>();
        for (int i = 0; i < newReports.Count(); i++)
        {
            dynamic oldReport = oldReports.ElementAt(i);
            Report newReport = newReports.ElementAt(i);

            if (oldVersion < 38)
            {
                newReport._ContentType = (int)oldReport.ContentType;
                newReport._ReasonType = (int)oldReport.ReasonType;
            }

            if (oldVersion < 40)
            {
                string authorId = (string)oldReport.Issuer.Id;
                newReport.Author = migration.NewRealm.All<GameUser>().First(u => u.Id == authorId);
                newReport.CreationDate = (DateTimeOffset)oldReport.Date;
            }
        }
        
        
        migration.OldRealm.DynamicApi.All("DailyLevel");
        IQueryable<DailyLevel> newDailyLevels = migration.NewRealm.All<DailyLevel>();
        for (int i = 0; i < newDailyLevels.Count(); i++)
        {
            DailyLevel newLevel = newDailyLevels.ElementAt(i);

            if (oldVersion < 49)
            {
                newLevel.CreationDate = newLevel.Date;
                newLevel.ModificationDate = newLevel.Date;
            }
        }
        
        IQueryable<GameIp> newIps = migration.NewRealm.All<GameIp>();
        for (int i = 0; i < newIps.Count(); i++)
        {
            GameIp newIp = newIps.ElementAt(i);

            if (oldVersion < 62)
            {
                migration.NewRealm.Remove(newIp);
            }
        }
        
        IQueryable<dynamic> oldEvents = migration.OldRealm.DynamicApi.All("GameEvent");
        IQueryable<GameEvent> events = migration.NewRealm.All<GameEvent>();

        for (int i = 0; i < events.Count(); i++)
        {
            dynamic oldEvent = oldEvents.ElementAt(i);
            GameEvent newEvent = events.ElementAt(i);
            
            if (oldVersion < 28)
            {
                // Added the Queue event type
                if (newEvent.EventType >= EventType.LevelQueue)
                {
                    newEvent.EventType += 1;
                }
            }

            if (oldVersion < 35)
            {
                // Renamed EventType to _EventType
                newEvent._EventType = (int)oldEvent.EventType;
            }

            if (oldVersion < 48)
            {
                // Renamed Date to CreationDate
                newEvent.CreationDate = (DateTimeOffset)oldEvent.Date;
            }

            if (oldVersion < 72)
            {
                if (oldEvent.ContentUser == null && oldEvent.ContentLevel == null &&
                    oldEvent.ContentLeaderboardEntry == null)
                {
                    migration.NewRealm.Remove(newEvent);
                    continue;
                }
                
                newEvent.Id = ObjectId.GenerateNewId();
                dynamic oldUser = oldEvent.Actor;
                
                string? oldUserId = (string?)oldUser.Id;
                if (oldUserId == null)
                {
                    migration.NewRealm.Remove(newEvent);
                    continue;
                }

                dynamic oldContentLevel = oldEvent.ContentLevel;
                string? oldContentLevelId = (string?)oldContentLevel?.Id;
                
                dynamic oldContentUser = oldEvent.ContentUser;
                string? oldContentUserId = (string?)oldContentUser?.Id;
                
                dynamic oldContentLeaderboardEntry = oldEvent.ContentLeaderboardEntry;
                string? oldContentLeaderboardEntryId = (string?)oldContentLeaderboardEntry?.Id;
                ObjectId newLeaderboardEntryId = ObjectId.Empty;
                if (oldContentLeaderboardEntryId != null)
                {
                    string id = oldContentLeaderboardEntryId;
                    id = new string(id.Where(c => c != '-').Take(24).ToArray());
                    newLeaderboardEntryId = ObjectId.Parse(id);
                }

                bool remove = false;
                switch ((int)oldEvent._EventType)
                {
                    case 0: // publishing
                    case 1: // like
                    case 2: // queue
                        if (oldContentLevelId == null ||
                            newLevels.FirstOrDefault(l => l.Id == oldContentLevelId) == null)
                            remove = true;
                        break;
                    case 3: // follow
                        if (oldContentUserId == null || newUsers.FirstOrDefault(l => l.Id == oldContentUserId) == null)
                            remove = true;
                        break;
                    case 4: // score submission

                        if (oldContentLevelId == null || newLevels.FirstOrDefault(l => l.Id == oldContentLevelId) == null || 
                            oldContentLeaderboardEntryId == null || newLeaderboardEntries.FirstOrDefault(e => e.Id == newLeaderboardEntryId) == null)
                            remove = true;
                        break;
                }

                if (remove)
                {
                    migration.NewRealm.Remove(newEvent);
                    continue;
                }
                
                switch ((int)oldEvent._EventType)
                {
                    case 0: // publishing
                        newEvent.EventType = EventType.LevelPublish;
                        newEvent.DataType = EventDataType.Level;
                        newEvent.DataId = oldContentLevelId!;
                        break;
                    case 1: // like
                        newEvent.EventType = EventType.LevelLike;
                        newEvent.DataType = EventDataType.Level;
                        newEvent.DataId = oldContentLevelId!;
                        break;
                    case 2: // queue
                        newEvent.EventType = EventType.LevelQueue;
                        newEvent.DataType = EventDataType.Level;
                        newEvent.DataId = oldContentLevelId!;
                        break;
                    case 3: // follow
                        newEvent.EventType = EventType.UserFollow;
                        newEvent.DataType = EventDataType.User;
                        newEvent.DataId = oldContentUserId!;
                        break;
                    case 4: // score submission
                        newEvent.EventType = EventType.ScoreSubmission;
                        newEvent.DataType = EventDataType.LeaderboardEntry;
                        newEvent.DataId = newLeaderboardEntryId.ToString()!;
                        break;
                    case 5: // account registration
                        newEvent.EventType = EventType.AccountRegistration;
                        newEvent.DataType = EventDataType.User;
                        newEvent.DataId = oldUserId;
                        break;
                }
            }
        }
    }
}