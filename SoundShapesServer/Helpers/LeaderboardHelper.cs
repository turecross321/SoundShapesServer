using SoundShapesServer.Requests.Game;

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
                    response.Notes = int.Parse(value);
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

    public static int CalculateEntryPlacement(int totalEntries, int from, int index, bool descending, bool startFromZero)
    {
        return descending ? 
            totalEntries - (from + index) - (startFromZero ? 0 : 1) : 
            from + index + (startFromZero ? 0 : 1);
    }
}