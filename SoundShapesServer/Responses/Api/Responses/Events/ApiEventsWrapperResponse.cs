using SoundShapesServer.Database;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Events;

namespace SoundShapesServer.Responses.Api.Responses.Events;

public class ApiEventsWrapperResponse : ApiResponse<ApiEventsWrapper>
{
    public ApiEventsWrapperResponse(GameDatabaseContext database, PaginatedList<GameEvent> events) : base(
        new ApiEventsWrapper(database, events))
    {
        ListInformation = new ApiListInformation
        {
            TotalItems = events.TotalItems,
            NextPageIndex = PaginationHelper.GetNextToken(events.TotalItems, events.From, events.Items.Count())
        };
    }

    public ApiListInformation? ListInformation { get; set; }
}