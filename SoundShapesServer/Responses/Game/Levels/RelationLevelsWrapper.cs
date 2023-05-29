using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Levels;

public class RelationLevelsWrapper
{
    public RelationLevelsWrapper(GameLevel[] levels, GameUser user, int totalLevels, int from, int count)
    {
        Levels = levels.Select(l => new RelationLevelResponse(user, l)).ToArray();
        TotalLevels = totalLevels;
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalLevels, from, count);
    }

    public RelationLevelsWrapper()
    {
        Levels = Array.Empty<RelationLevelResponse>();
        TotalLevels = 0;
    }

    [JsonProperty("items")] public RelationLevelResponse[] Levels { get;}
    [JsonProperty("count")] public int TotalLevels { get; }
    
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] 
    public int? NextToken { get; set; }
}