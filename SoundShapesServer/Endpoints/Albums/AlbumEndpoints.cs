using System.Net;
using Bunkum.CustomHttpListener.Parsing;
using Bunkum.HttpServer;
using Bunkum.HttpServer.Endpoints;
using Bunkum.HttpServer.Responses;
using SoundShapesServer.Authentication;
using SoundShapesServer.Database;
using SoundShapesServer.Responses.Albums;
using SoundShapesServer.Types;
using SoundShapesServer.Types.Albums;

namespace SoundShapesServer.Endpoints.Albums;

public class AlbumEndpoints : EndpointGroup
{
    [Endpoint("/otg/~albums/~link:*.page", ContentType.Json)]
    public AlbumsWrapper GetAlbums(RequestContext context, RealmDatabaseContext database, GameSession token)
    {
        int from = int.Parse(context.QueryString["from"] ?? "0");
        int count = int.Parse(context.QueryString["count"] ?? "9");

        return database.GetAlbums(token.id, from, count);
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