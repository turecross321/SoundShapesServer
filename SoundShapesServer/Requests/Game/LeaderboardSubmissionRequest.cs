namespace SoundShapesServer.Requests.Game;

public class LeaderboardSubmissionRequest
{
    public int Deaths { get; set; }
    public long PlayTime { get; set; }
    public int Golded { get; set; }
    public int Notes { get; set; }
    public long Score { get; set; }
    
    // is true by default (completed) because campaign score submissions don't use the completion parameter
    public bool Completed { get; set; } = true; 
}