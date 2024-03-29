using SoundShapesServer.Attributes;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Punishments;

public class PunishmentFilters: IFilters
{
    [FilterProperty("author", "Filter out punishments that were not authored by user with specified ID.")]
    public GameUser? Author { get; init; }
    [FilterProperty("recipient", "Filter out punishments where the recipient is not the user with specified ID.")]
    public GameUser? Recipient { get; init; }
    [FilterProperty("revoked", "Filter out punishments based on whether the punishment has been revoked or not.")]
    public bool? Revoked { get; init; }
}