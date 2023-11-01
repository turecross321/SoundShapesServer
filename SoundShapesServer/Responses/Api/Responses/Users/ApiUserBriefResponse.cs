using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Api.Responses.Users;

public class ApiUserBriefResponse : IApiResponse, IDataConvertableFrom<ApiUserBriefResponse, GameUser>
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required int PermissionsType { get; set; }
    public required int FollowersCount { get; set; }
    public required int FollowingCount { get; set; }
    public required int PublishedLevelsCount { get; set; }

    public static ApiUserBriefResponse FromOld(GameUser old)
    {
        return new ApiUserBriefResponse
        {
            Id = old.Id,
            Username = old.Username,
            PermissionsType = (int)old.PermissionsType,
            FollowersCount = old.FollowersRelations.Count(),
            FollowingCount = old.FollowingRelations.Count(),
            PublishedLevelsCount = old.Levels.Count()
        };
    }

    public static IEnumerable<ApiUserBriefResponse> FromOldList(IEnumerable<GameUser> oldList)
    {
        return oldList.Select(FromOld);
    }
}