using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Punishments;

public class PunishmentFilters
{
    public GameUser? Author { get; set; }
    public GameUser? Recipient { get; set; }
    public bool? Revoked { get; set; }
}