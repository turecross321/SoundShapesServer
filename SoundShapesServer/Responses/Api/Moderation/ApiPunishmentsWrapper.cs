using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiPunishmentsWrapper
{
    public ApiPunishmentsWrapper(IQueryable<Punishment> punishments, int from, int count)
    {
        // todo: fix trhis SHIT
        Punishment[] paginatedPunishments = PaginationHelper.PaginatePunishments(punishments, from, count);

        Punishments = paginatedPunishments.Select(p => new ApiPunishmentResponse(p)).ToArray();
        Count = punishments.Count();
    }

    public ApiPunishmentResponse[] Punishments { get; }
    public int Count { get; }
}