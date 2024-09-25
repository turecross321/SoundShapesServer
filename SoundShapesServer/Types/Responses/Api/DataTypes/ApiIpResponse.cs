using SoundShapesServer.Types.Database;
using SoundShapesServer.Types.Responses.Api.ApiTypes;

namespace SoundShapesServer.Types.Responses.Api.DataTypes;

public record ApiIpResponse : IApiDbResponse<DbIp, ApiIpResponse>
{
    public required Guid Id { get; init; }
    public required ApiMinimalUserResponse User { get; init; }
    public required string IpAddress { get; init; }
    public required DateTimeOffset CreationDate { get; init; }
    public required DateTimeOffset? AuthorizedDate { get; set; }
    public required bool Authorized { get; init; }
    public required bool? OneTimeUse { get; set; }
    
    public static ApiIpResponse FromDb(DbIp value)
    {
        return new ApiIpResponse
        {
            User = ApiMinimalUserResponse.FromDb(value.User),
            IpAddress = value.IpAddress,
            CreationDate = value.CreationDate,
            AuthorizedDate = value.AuthorizedDate,
            Authorized = value.Authorized,
            OneTimeUse = value.OneTimeUse,
            Id = value.Id,
        };
    }
}