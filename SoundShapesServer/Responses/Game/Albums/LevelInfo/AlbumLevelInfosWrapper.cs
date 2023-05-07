using Newtonsoft.Json;
using SoundShapesServer.Types.Albums;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Albums.LevelInfo;

public class AlbumLevelInfosWrapper
{
    public AlbumLevelInfosWrapper(GameUser user, GameAlbum album, GameLevel[] levels, int? previousToken, int? nextToken)
    {
        Items = levels.Select(level => new AlbumLevelInfoResponse(user, album, level)).ToArray();
        PreviousToken = previousToken;
        NextToken = nextToken;
    }

    [JsonProperty("items")] public AlbumLevelInfoResponse[] Items { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}