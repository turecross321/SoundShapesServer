using SoundShapesServer.Requests.Game;
using SoundShapesServer.Responses.Api;
using SoundShapesServer.Responses.Api.Levels;
using SoundShapesServer.Responses.Game.Leaderboards;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Helpers;

public static class LeaderboardHelper
{
    public static LeaderboardEntriesWrapper LeaderboardEntriesToWrapper(IQueryable<LeaderboardEntry> entries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(entries.Count(), from, count);
        
        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(entries, from, count);

        List<LeaderboardEntryResponse> entryResponses = new ();

        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            LeaderboardEntryResponse? levelResponse = LeaderboardEntryToResponse(paginatedEntries[i], i + from + 1);
            entryResponses.Add(levelResponse);
        }

        LeaderboardEntriesWrapper response = new()
        { 
            Entries = entryResponses.ToArray(),
            NextToken = nextToken,
            PreviousToken = previousToken
        };

        return response;
    }

    public static LeaderboardEntryResponse LeaderboardEntryToResponse(LeaderboardEntry entry, int position)
    {
        return new LeaderboardEntryResponse
        {
            Position = position,
            Entrant = UserHelper.UserToUserResponse(entry.User),
            Score = entry.Score
        };
    }

    public static LeaderboardSubmissionRequest DeSerializeSubmission(string str)
    {
        LeaderboardSubmissionRequest response = new LeaderboardSubmissionRequest();
        
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
                    response.TokenCount = int.Parse(value);
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
    
    // API

    public static ApiLeaderboardEntryWrapper LeaderboardEntriesToApiWrapper(IQueryable<LeaderboardEntry> entries, int from,
        int count)
    {
        LeaderboardEntry[] paginatedEntries = PaginationHelper.PaginateLeaderboardEntries(entries, from, count);

        List<ApiLeaderboardEntryResponse> entryResponses = new ();

        for (int i = 0; i < paginatedEntries.Length; i++)
        {
            ApiLeaderboardEntryResponse levelResponse = LeaderboardEntryToApiResponse(paginatedEntries[i], i + from);
            entryResponses.Add(levelResponse);
        }

        ApiLeaderboardEntryWrapper response = new()
        {
            Entries = entryResponses.ToArray(),
            Count = entries.Count()
        };

        return response;
    } 
    
    public static ApiLeaderboardEntryResponse LeaderboardEntryToApiResponse(LeaderboardEntry entry, int position)
    {
        return new ApiLeaderboardEntryResponse
        {
            Position = position,
            UserId = entry.User.Id,
            Username = entry.User.Username,
            Score = entry.Score,
            PlayTime = entry.PlayTime,
            Tokens = entry.Tokens,
            Deaths = entry.Deaths,
            Date = entry.Date
        };
    }
}