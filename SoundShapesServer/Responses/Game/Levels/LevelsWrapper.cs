using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelsWrapper
{
    public LevelsWrapper(IQueryable<GameLevel> levels, GameUser user, int from, int count, LevelOrderType order)
    {
        IQueryable<GameLevel> orderedLevels = LevelHelper.OrderLevels(levels, order, true);
        GameLevel[] paginatedLevels = PaginationHelper.PaginateLevels(orderedLevels, from, count);
        LevelResponse[] levelResponses = paginatedLevels.Select(l => new LevelResponse(l, user)).ToArray();
        
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(orderedLevels.Count(), from, count);

        Levels = levelResponses.ToArray();
        LevelCount = levelResponses.Length;
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