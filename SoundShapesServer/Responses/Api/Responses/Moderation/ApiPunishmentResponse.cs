using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Responses.Moderation;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishmentResponse : IApiResponse
{
    [Obsolete("Empty constructor for deserialization.", true)]
    public ApiPunishmentResponse() {}
    
    public ApiPunishmentResponse(Punishment punishment)
    {
        Id = punishment.Id;
        Recipient = new ApiUserBriefResponse(punishment.Recipient);
        PunishmentType = punishment.PunishmentType;
        Reason = punishment.Reason;
        Revoked = punishment.Revoked;
        Author = new ApiUserBriefResponse(punishment.Author);
        CreationDate = punishment.CreationDate.ToUnixTimeSeconds();
        ModificationDate = punishment.ModificationDate.ToUnixTimeSeconds();
        ExpiryDate = punishment.ExpiryDate.ToUnixTimeSeconds();
        RevokeDate = punishment.RevokeDate?.ToUnixTimeSeconds();
    }

    public string Id { get; }
    public ApiUserBriefResponse Recipient { get; set; }
    public PunishmentType PunishmentType { get; set; }
    public string Reason { get; set; }
    public bool Revoked { get; set; }
    public ApiUserBriefResponse Author { get; set; }
    public long CreationDate { get; }
    public long ModificationDate { get; }
    public long ExpiryDate { get; set; }
    public long? RevokeDate { get; set; }
}