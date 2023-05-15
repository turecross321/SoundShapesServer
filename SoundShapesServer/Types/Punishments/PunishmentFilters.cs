using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types.Punishments;

public class PunishmentFilters
{
    public PunishmentFilters(GameUser? author, GameUser? recipient, bool? revoked)
    {
        Author = author;
        Recipient = recipient;
        Revoked = revoked;
    }
    public GameUser? Author { get; set; }
    public GameUser? Recipient { get; set; }
    public bool? Revoked { get; set; }
}