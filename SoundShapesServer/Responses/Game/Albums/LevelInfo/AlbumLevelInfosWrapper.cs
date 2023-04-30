using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfosWrapper
{
    public AlbumLevelInfosWrapper(GameUser user, GameAlbum album, IQueryable<GameLevel> levels, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(levels.Count(), from, count);
        
        Items = levels.Select(level => new AlbumLevelInfoResponse(user, album, level)).ToArray();
        PreviousToken = previousToken;
        NextToken = nextToken;
    }

    [JsonProperty("items")] public AlbumLevelInfoResponse[] Items { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}