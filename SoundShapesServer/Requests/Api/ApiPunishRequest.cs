// ReSharper disable UnassignedGetOnlyAutoProperty

using SoundShapesServer.Types.Punishments;

#pragma warning disable CS8618

namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishRequest
{
    public string UserId { get; set; }
    public PunishmentType PunishmentType { get; set; }
    public string Reason { get; set; }
    public DateTimeOffset ExpiryDate { get; set; }
}