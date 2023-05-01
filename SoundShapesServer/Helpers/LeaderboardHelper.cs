using SoundShapesServer.Database;
using SoundShapesServer.Requests.Game;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LeaderboardHelper
{
    public static LeaderboardSubmissionRequest DeSerializeSubmission(string str)
    {
        LeaderboardSubmissionRequest response = new();
        
        string[] queries = str.Split("&");

        foreach (string? query in queries)
        {
            string[] nameAndValue = query.Split("=");
            string name = nameAndValue[0];
            string value = nameAndValue[1];

            switch (name)
            {
                case "deaths":
                    response.Deaths = int.Parse(value);
                    break;
                case "playTime":
                    response.PlayTime = long.Parse(value);
                    break;
                case "golded":
                    response.Golded = int.Parse(value);
                    break;
                case "tokenCount":
                    response.Tokens = int.Parse(value);
                    break;
                case "score":
                    response.Score = long.Parse(value);
                    break;
                case "completed":
                    response.Completed = int.Parse(value) == 1;
                    break;
            }
            
        }

        return response;
    }
    
    public static LeaderboardEntry? GetBestEntry(IQueryable<LeaderboardEntry> entries,
        GameUser user)
    {
        if (entries == null) throw new ArgumentNullException(nameof(entries));
        
        LeaderboardEntry? entry =
            entries
                .AsEnumerable()
                .OrderByDescending(e=>e.Score)
                .FirstOrDefault(e => e.Completed && e.User.Id == user.Id);

        return entry;
    }

    public static int GetEntryPlacement(IQueryable<LeaderboardEntry> entries, LeaderboardEntry entry)
    {
        return entries.ToList().IndexOf(entry);
    }
    public static int GetTotalLeaderboardPlacements(RealmDatabaseContext database, GameUser user)
    {
        int count = 0;
        
        // Get all the unique LevelIds for the user's LeaderboardEntries
        string[] levelIds = user.LeaderboardEntries.Select(entry => entry.LevelId).Distinct().ToArray();

        foreach (string levelId in levelIds)
        {
            IQueryable<LeaderboardEntry> entries = database.GetLeaderboardEntriesOnLevel(levelId);

            LeaderboardEntry? bestEntry = GetBestEntry(entries, user);
            if (bestEntry == null) continue;
            
            count += GetEntryPlacement(entries, bestEntry) - entries.Count();
        }

        return count;
    }
}