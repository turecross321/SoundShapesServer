using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelsWrapper
{
    public LevelsWrapper(IQueryable<GameLevel> levels, GameUser user, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(levels.Count(), from, count);
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(levels, from, count);

        List<LevelResponse> levelResponses = paginatedLevels.Select(t => new LevelResponse(t, user)).ToList();

        Levels = levelResponses.ToArray();
        LevelCount = levelResponses.Count;
        NextToken = nextToken;
        PreviousToken = previousToken;
    }

    public LevelsWrapper()
    {
        Levels = Array.Empty<LevelResponse>();
        LevelCount = 0;
    }

    [JsonProperty("items")] public LevelResponse[] Levels { get;}
    [JsonProperty("count")] public int LevelCount { get; }
    
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? NextToken { get; set; }
}