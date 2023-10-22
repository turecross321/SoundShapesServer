using Newtonsoft.Json;
using SoundShapesServer.Extensions;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Game.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Levels;
using SoundShapesServer.Types.Users;
using static SoundShapesServer.Helpers.IdHelper;

namespace SoundShapesServer.Responses.Game.Levels;

public class LevelTargetResponse : IResponse
{
    public LevelTargetResponse(GameLevel level, GameUser accessor)
    {
        Id = FormatLevelId(level.Id);
        LatestVersion = new LevelVersionResponse(level);
        Completed = level.UniqueCompletions.Contains(accessor);
        Liked = level.Likes.AsEnumerable().Select(r=>r.User).Contains(accessor);
        
        if (level.HasUserAccess(accessor))
        {
            Author = new UserTargetResponse(level.Author);
            Metadata = new LevelMetadataResponse(level);
        }
        else
        {
            Author = new UserTargetResponse();
            Metadata = LevelMetadataResponse.PrivateLevel();
        }
    }

    [JsonProperty("id")] public string Id { get; }
    [JsonProperty("author")] public UserTargetResponse Author { get;  }
    [JsonProperty("latestVersion")] public LevelVersionResponse LatestVersion { get; }
    [JsonProperty("type")] public string Type = ContentHelper.GetContentTypeString(GameContentType.Level);
    [JsonProperty("completed")] public bool Completed { get; }
    [JsonProperty("liked")] public bool Liked { get; }
    [JsonProperty("metadata")] public LevelMetadataResponse Metadata { get; }
}