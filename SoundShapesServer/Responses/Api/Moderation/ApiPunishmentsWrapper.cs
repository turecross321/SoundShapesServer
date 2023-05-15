using SoundShapesServer.Helpers;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Moderation;

public class ApiPunishmentsWrapper
{
    public ApiPunishmentsWrapper(IEnumerable<Punishment> punishments, int totalPunishments)
    {
        Punishments = punishments.Select(p => new ApiPunishmentResponse(p)).ToArray();
        Count = totalPunishments;
    }

    public ApiPunishmentResponse[] Punishments { get; }
    public int Count { get; }
}