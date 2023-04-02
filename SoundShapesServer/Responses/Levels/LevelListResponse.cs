namespace SoundShapesServer.Responses.Levels;
public class LevelListResponse
{
    public List<LevelResponse> items { get; set; }
    public int nextToken { get; set; }
    public int count { get; set; }
}