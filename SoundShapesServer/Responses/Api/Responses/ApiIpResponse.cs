using SoundShapesServer.Responses.Api.Framework;
using SoundShapesServer.Types;

namespace SoundShapesServer.Responses.Api.Responses;

// ReSharper disable once ClassNeverInstantiated.Global
public class ApiIpResponse : IApiResponse, IDataConvertableFrom<ApiIpResponse, GameIp>
{
    public string IpAddress { get; set; }
    public bool Authorized { get; set; }
    public bool OneTimeUse { get; set; }
    public DateTimeOffset CreationDate { get; set; }
    public DateTimeOffset ModificationDate { get; set; }

    public static ApiIpResponse FromOld(GameIp old)
    {
        return new ApiIpResponse
        {
            IpAddress = old.IpAddress,
            Authorized = old.Authorized,
            OneTimeUse = old.OneTimeUse,
            CreationDate = old.CreationDate,
            ModificationDate = old.ModificationDate
        };
    }

    public static IEnumerable<ApiIpResponse> FromOldList(IEnumerable<GameIp> oldList)
    {
        return oldList.Select(FromOld);
    }
}