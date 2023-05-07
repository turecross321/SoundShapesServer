using Realms;
using SoundShapesServer.Types.Users;

namespace SoundShapesServer.Types;

public class Report : RealmObject
{
        [PrimaryKey]
    [Required] public string Id { get; init; } = "";
    public string ContentId { get; init; } = "";
    public string ContentType { get; init; } = "";
    public int ReportReasonId { get; init; }
    public DateTimeOffset Issued { get; init; }
    public GameUser Issuer { get; init; } = new();
}