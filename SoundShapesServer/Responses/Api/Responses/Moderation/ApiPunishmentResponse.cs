using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Responses.Api.Responses.Users;
using SoundShapesServer.Types.Punishments;

namespace SoundShapesServer.Responses.Api.Responses.Moderation;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiPunishmentResponse : IApiResponse, IDataConvertableFrom<ApiPunishmentResponse, Punishment>
{
    public required string Id { get; set; }
    public required ApiUserBriefResponse Recipient { get; set; }
    public required PunishmentType PunishmentType { get; set; }
    public required string Reason { get; set; }
    public required bool Revoked { get; set; }
    public required ApiUserBriefResponse Author { get; set; }
    public required DateTimeOffset CreationDate { get; set; }
    public required DateTimeOffset ModificationDate { get; set; }
    public required DateTimeOffset ExpiryDate { get; set; }
    public required DateTimeOffset? RevokeDate { get; set; }

    public static ApiPunishmentResponse FromOld(Punishment old)
    {
        return new ApiPunishmentResponse
        {
            Id = old.Id.ToString()!,
            Recipient = ApiUserBriefResponse.FromOld(old.Recipient),
            PunishmentType = old.PunishmentType,
            Reason = old.Reason,
            Revoked = old.Revoked,
            Author = ApiUserBriefResponse.FromOld(old.Author),
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate,
            ExpiryDate = old.ExpiryDate,
            RevokeDate = old.RevokeDate
        };
    }

    public static IEnumerable<ApiPunishmentResponse> FromOldList(IEnumerable<Punishment> oldList)
    {
        return oldList.Select(FromOld);
    }
}