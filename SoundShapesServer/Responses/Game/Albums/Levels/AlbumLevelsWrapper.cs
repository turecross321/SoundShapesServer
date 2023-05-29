using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.Levels;

public class AlbumLevelsWrapper
{
    public AlbumLevelsWrapper(GameUser user, GameAlbum album, GameLevel[] levels, int totalEntries, int from, int count)
    {
        (int? previousToken, int? nextToken) = PaginationHelper.GetPageTokens(totalEntries, from, count);

        Levels = levels.Select(t => new AlbumLevelResponse(album, t, user)).ToArray();
        PreviousToken = previousToken;
        NextToken = nextToken;
    }

    [JsonProperty("items")] public AlbumLevelResponse[] Levels { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}