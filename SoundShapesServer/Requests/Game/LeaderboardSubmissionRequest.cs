namespace SoundShapesServer.Requests.Game;

public class LeaderboardSubmissionRequest
{
    public int Deaths { get; set; }
    public int PlayTime { get; set; }
    public int Golded { get; set; }
    public int TokenCount { get; set; }
    public long Score { get; set; }
    public int Completed { get; set; } = 1; // is 1 by default (completed) because campaign score submissions don't use the completion parameter
}