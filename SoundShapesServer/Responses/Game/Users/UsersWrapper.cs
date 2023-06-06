using Newtonsoft.Json;
using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Responses.Game.Users;

public class UsersWrapper
{
    public UsersWrapper(GameUser user, GameUser[] users, int totalUsers, int from, int count)
    {
        Users = users.Select(t => new UserBriefResponse(user, t)).ToArray();
        (PreviousToken, NextToken) = PaginationHelper.GetPageTokens(totalUsers, from, count);
    }

    [JsonProperty("items")] public UserBriefResponse[] Users { get; set; }
    [JsonProperty("previousToken", NullValueHandling=NullValueHandling.Ignore)] public int? PreviousToken { get; set; }
    [JsonProperty("nextToken", NullValueHandling=NullValueHandling.Ignore)] public int? NextToken { get; set; }
}