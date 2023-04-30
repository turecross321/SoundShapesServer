using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Game.Following;

public class FollowingUsersWrapper
{
    public FollowingUsersWrapper(GameUser user, IQueryable<GameUser> users, int from, int count)
    {
        (int? nextToken, int? previousToken) = PaginationHelper.GetPageTokens(users.Count(), from, count);

        Users = users.Select(t => new FollowingUserResponse(user, t)).ToArray();
        NextToken = nextToken;
        PreviousToken = previousToken;
    }

    [JsonProperty("items")] public FollowingUserResponse[] Users { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}