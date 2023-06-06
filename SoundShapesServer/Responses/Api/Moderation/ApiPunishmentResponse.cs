using SoundShapesServer.Responses.Api.Users;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Moderation;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishmentResponse
{
    public ApiPunishmentResponse(Punishment punishment)
    {
        Id = punishment.Id;
        Recipient = new ApiUserBriefResponse(punishment.Recipient);
        PunishmentType = punishment.PunishmentType;
        Reason = punishment.Reason;
        Revoked = punishment.Revoked;
        Author = new ApiUserBriefResponse(punishment.Author);
        CreationDate = punishment.CreationDate;
        ExpiryDate = punishment.ExpiryDate;
        RevokeDate = punishment.RevokeDate;
    }

    public string Id { get; }
    public ApiUserBriefResponse Recipient { get; set; }
    public PunishmentType PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public DateTimeOffset CreationDate { get; }
    public DateTimeOffset ExpiryDate { get; set; }
    public DateTimeOffset? RevokeDate { get; set; }
}