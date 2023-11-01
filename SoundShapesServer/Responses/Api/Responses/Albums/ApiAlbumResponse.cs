using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Albums;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SoundShapesServer.Responses.Api.Responses.Albums;

public class ApiAlbumResponse : IApiResponse, IDataConvertableFrom<ApiAlbumResponse, GameAlbum>
{
    public required string Id { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required string Name { get; set; }
    public required string LinerNotes { get; set; }
    public required int TotalLevels { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }

    public static ApiAlbumResponse FromOld(GameAlbum old)
    {
        return new ApiAlbumResponse
        {
            Id = old.Id.ToString()!,
            Author = ApiUserBriefResponse.FromOld(old.Author),
            Name = old.Name,
            LinerNotes = old.LinerNotes,
            TotalLevels = old.Levels.Count,
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate
        };
    }

    public static IEnumerable<ApiAlbumResponse> FromOldList(IEnumerable<GameAlbum> oldList)
    {
        return oldList.Select(FromOld);
    }
}