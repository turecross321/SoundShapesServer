namespace SoundShapesServer.Requests;

public class LeaderboardSubmissionRequest
{
    public int deaths { get; set; }
    public int playTime { get; set; }
    public int golded { get; set; }
    public long date { get; set; }
    public int tokenCount { get; set; }
    public long score { get; set; }
    public int completed { get; set; }
}