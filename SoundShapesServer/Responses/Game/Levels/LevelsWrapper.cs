using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Levels;
public class LevelsWrapper
{
    public LevelsWrapper(GameLevel[] levels, GameUser user, int totalLevels, int from, int count)
    {
        Levels = levels.Select(l => new LevelResponse(l, user)).ToArray();
        TotalLevels = totalLevels;
        
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalLevels, from, count);
    }

    public LevelsWrapper()
    {
        Levels = Array.Empty<LevelResponse>();
        TotalLevels = 0;
    }

    [JsonProperty("items")] public LevelResponse[] Levels { get;}
    [JsonProperty("count")] public int TotalLevels { get; }
    
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? NextToken { get; set; }
}