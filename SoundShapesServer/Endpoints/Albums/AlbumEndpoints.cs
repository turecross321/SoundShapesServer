using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Database;
using SoundShapesServer.Enums;
using SoundShapesServer.Helpers;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Responses.Levels;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Endpoints.Albums;

public class AlbumEndpoints : EndpointGroup
{
    // ?count=9&decorate=metadata&order=target.id:desc
    [Endpoint("/otg/~albums/~link:*.page", ContentType.Json)]
    public AlbumsWrapper GetAlbums(RequestContext context, RealmDatabaseContext database)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        return database.GetAlbums(from, count);
    }

    [Endpoint("/otg/~album:{albumId}/~link:*.page", ContentType.Json)]
    public Response GetAlbumLevels
        (RequestContext context, RealmDatabaseContext database, GameUser user, string albumId)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");
        string? order = context.QueryString["order"];

        GameAlbum? album = database.GetAlbumWithId(albumId);

        if (album == null) return HttpStatusCode.NotFound;

        if (order == "time:ascn") return new Response(database.AlbumLevels(user, album, from, count), ContentType.Json);

        return new Response(database.GetAlbumsLevelsInfo(user, album, from, count), ContentType.Json);
    }
}