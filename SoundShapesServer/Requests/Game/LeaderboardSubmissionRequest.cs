namespace SoundShapesServer.Requests.Game;

public class LeaderboardSubmissionRequest
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
    public int Deaths { get; set; }
    public long PlayTime { get; set; }
    public int Golded { get; set; }
    public int Notes { get; set; }
    public long Score { get; set; }
    
    // is true by default (completed) because campaign score submissions don't use the completion parameter
    public bool Completed { get; set; } = true; 
}