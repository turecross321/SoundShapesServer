using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using HtmlAgilityPack;
using SoundShapesServer.Responses.Game.Albums;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Helpers;

public static class AlbumHelper
{
    public static LinerNoteResponse[] XmlToLinerNotes(string xmlString)
    {
        List<LinerNoteResponse> linerNotes = new ();

        XmlDocument xmlDoc = new ();
        xmlDoc.LoadXml(xmlString);

        if (xmlDoc.DocumentElement == null) return linerNotes.ToArray();
        
        XmlNodeList elements = xmlDoc.DocumentElement.ChildNodes;
        
        foreach (XmlNode element in elements)
        {
            FontType? fontType = element.Name switch
            {
                "title" => FontType.Title,
                "header" => FontType.Header,
                "normal" => FontType.Normal,
                _ => null
            };
            
            if (fontType == null) continue;
            
            LinerNoteResponse linerNote = new ((FontType)fontType, element.InnerText);

            linerNotes.Add(linerNote);
        }

        return linerNotes.ToArray();
    }

    public static AlbumResourceType GetAlbumResourceTypeFromString(string resourceTypeString)
    {
        return resourceTypeString switch
        {
            "thumbnail" => AlbumResourceType.Thumbnail,
            "sidePanel" => AlbumResourceType.SidePanel,
            _ => throw new ArgumentOutOfRangeException(nameof(resourceTypeString), resourceTypeString, null)
        };
    }

    public static string GetStringFromAlbumResourceType(AlbumResourceType resourceType)
    {
        return resourceType switch
        {
            AlbumResourceType.Thumbnail => "thumbnail",
            AlbumResourceType.SidePanel => "sidePanel",
            _ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, null)
        };
    }
}