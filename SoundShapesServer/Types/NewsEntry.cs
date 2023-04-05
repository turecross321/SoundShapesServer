using System.Reflection;
using JetBrains.Annotations;
using MongoDB.Bson;
using Realms;

namespace SoundShapesServer.Types;

public class NewsEntry : RealmObject
{
    public string language { get; set; } = "global";
    public string title { get; set; }
    public string text { get; set; }
    public string fullText { get; set; }
    public string url { get; set; }
}