using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowingUsersWrapper
{
    public FollowingUsersWrapper(GameUser user, GameUser[] users, int totalUsers, int from, int count)
    {
        Users = users.Select(t => new FollowingUserResponse(user, t)).ToArray();
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalUsers, from, count);
    }

    [JsonProperty("items")] public FollowingUserResponse[] Users { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}