using SoundShapesServer.Types.Punishments;
// ReSharper disable UnassignedGetOnlyAutoProperty
#pragma warning disable CS8618

namespace SoundShapesServer.Requests.Api;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishRequest
{
    public string UserId { get; }
    public PunishmentType PunishmentType { get; }
    public string Reason { get; }
    public DateTimeOffset ExpiresAt { get; }
}