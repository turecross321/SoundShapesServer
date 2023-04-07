namespace SoundShapesServer.Requests;

public class LeaderboardSubmissionRequest
{
    public int deaths { get; set; }
    public int playTime { get; set; }
    public int golded { get; set; }
    public int tokenCount { get; set; }
    public long score { get; set; }
    public int completed { get; set; } = 1; // is 1 by default (completed) because campaign score submissions don't use the completion parameter
}