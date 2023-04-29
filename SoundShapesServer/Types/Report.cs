using MongoDB.Bson;
using Realms;
using SoundShapesServer.Types.Levels;

namespace SoundShapesServer.Types;

public class Report : RealmObject
{
    public string Id { get; set;  }
    public string ContentId { get; set; }
    public int ContentType { get; set; }
    public int ReportReasonId { get; set; }
    public DateTimeOffset Issued { get; set; }
    public GameUser Issuer { get; set; }
}