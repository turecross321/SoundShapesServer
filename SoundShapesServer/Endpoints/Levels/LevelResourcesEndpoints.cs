using System.Diagnostics;
using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Enums;

namespace SoundShapesServer.Endpoints.Levels;

public class LevelResourcesEndpoints : EndpointGroup
{
    private Response GetResource(RequestContext context, string levelId, string fileTypeString)
    {
        string fileName = levelId;


        if (!Enum.TryParse(fileTypeString, true, out FileType fileType))
            return HttpStatusCode.BadRequest;
        
        string fileExtension = "";
        
        switch (fileType)
        {
            case FileType.Image:
                fileExtension = ".png";
                break;
            case FileType.Level:
                fileExtension = ".vnd.soundshapes.level";
                break;
            case FileType.Sound:
                fileExtension = ".vnd.soundshapes.sound";
                break;
        }

        fileName += fileExtension;
        
        if (!context.DataStore.ExistsInStore(fileName))
            return HttpStatusCode.NotFound;

        if (!context.DataStore.TryGetDataFromStore(fileName, out byte[]? data))
            return HttpStatusCode.InternalServerError;

        Debug.Assert(data != null);
        return new Response(data, ContentType.BinaryData);
    }

    [Endpoint("/otg/~level:{levelId}/~version:{versionId}/~content:{fileTypeString}/data.get")]
    [Authentication(false)]
    public Response GetLevelResource(RequestContext context, string levelId, string versionId, string fileTypeString)
    {
        return GetResource(context, levelId, fileTypeString);
    }
}