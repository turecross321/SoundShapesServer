using SoundShapesServer.Attributes;

namespace SoundShapesServer.Types.Punishments;

public enum PunishmentOrderType
{
    [OrderType("creationDate", "Creation date")]
    CreationDate,
    [OrderType("expiryDate", "Expiry date")]
    ExpiryDate,
}