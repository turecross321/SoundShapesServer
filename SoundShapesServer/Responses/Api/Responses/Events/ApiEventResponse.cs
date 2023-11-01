using Newtonsoft.Json;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Responses.Api.Responses.Events;

[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class ApiEventResponse : IApiResponse, IDataConvertableFrom<ApiEventResponse, GameEvent>
{
    public required string Id { get; set; }
    public required EventType EventType { get; set; }
    public required ApiUserBriefResponse Actor { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required PlatformType PlatformType { get; set; }
    public required EventDataType DataType { get; set; }

    public static IEnumerable<ApiEventResponse> FromOldList(
        IEnumerable<GameEvent> oldList)
    {
        return oldList.Select(FromOld);
    }

    public static ApiEventResponse FromOld(GameEvent old)
    {
        return new ApiEventResponse
        {
            Id = old.Id.ToString()!,
            EventType = old.EventType,
            Actor = ApiUserBriefResponse.FromOld(old.Actor),
            DataType = old.DataType,
            CreationDate = old.CreationDate,
            PlatformType = old.PlatformType
        };
    }
}